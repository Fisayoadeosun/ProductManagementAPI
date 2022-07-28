using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductManagementAPI.Data;
using ProductManagementAPI.Data.Model;
using ProductManagementAPI.Data.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<Admin> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ProductMgtDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AdminController(IHttpContextAccessor contextAccessor, UserManager<Admin> userManager, RoleManager<IdentityRole> roleManager, 
            ProductMgtDbContext context, IConfiguration configuration, TokenValidationParameters tokenValidationParameters)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
            _tokenValidationParameters = tokenValidationParameters;
        }

        [HttpPost("Register-Admin")]
        public async Task<IActionResult> Register([FromBody]RegisterAdminVM registerAdminVM)
        {
            //registering admin details
            if (!ModelState.IsValid)
            {
                return BadRequest("Please provide all required field");
            }
            //check if admin already exist
            var adminExists = await _userManager.FindByEmailAsync(registerAdminVM.EmailAddress);
            if(adminExists != null)
            {
                return BadRequest($"Admin {registerAdminVM.EmailAddress} already exists");
            }
            //add
            Admin newAdmin = new Admin()
            {
                FirstName = registerAdminVM.FirstName,
                LastName = registerAdminVM.LastName,
                EmailAddress = registerAdminVM.EmailAddress,
                UserName = registerAdminVM.Username,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(newAdmin, registerAdminVM.Password);

            if (result.Succeeded)
            {
                return Ok("Admin Created");
            }
            return BadRequest("New Admin could not be created");
        }

        [HttpPost("login-admin")]
        public async Task<IActionResult> Login([FromBody] LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please provide all required fields");
            }
            var adminExists = await _userManager.FindByEmailAsync(loginVM.EmailAddress);

            if (adminExists != null && await _userManager.CheckPasswordAsync(adminExists, loginVM.Password))
            {
                var tokenValue = await GenerateJWTTokenAsync(adminExists, null);

                return Ok(tokenValue);
            };

            return Unauthorized();
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refreshtoken([FromBody] TokenRequestVM tokenRequestVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please provide all required fields");
            }
            var result = await VerifyAndGenerateTokenAsync(tokenRequestVM);

            return Ok(result);
        }

        private async Task<AuthResultVM>VerifyAndGenerateTokenAsync(TokenRequestVM tokenRequestVM)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequestVM.RefreshToken);
            var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);

            try
            {
                var tokenCheckResult = jwtTokenHandler.ValidateToken(tokenRequestVM.Token, _tokenValidationParameters, out var validatedToken);

                return await GenerateJWTTokenAsync(dbUser, storedToken);
            }
            catch (SecurityTokenExpiredException)
            {
                if (storedToken.DateExpire >= DateTime.UtcNow)
                {
                    return await GenerateJWTTokenAsync(dbUser, storedToken);
                }
                else
                {
                    return await GenerateJWTTokenAsync(dbUser, null);
                }
            }
        }

        private async Task<AuthResultVM> GenerateJWTTokenAsync(Admin admin, RefreshTokens rToken)
        {
            var AuthClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, admin.UserName),
                new Claim(ClaimTypes.NameIdentifier, admin.Id),
                new Claim(ClaimTypes.Role, admin.UserRole),
                new Claim(JwtRegisteredClaimNames.Email, admin.Email),
                new Claim(JwtRegisteredClaimNames.Sub, admin.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };


            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(20),
                claims: AuthClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            if (rToken != null)
            {
                var rTokenResponse = new AuthResultVM()
                {
                    Token = jwtToken,
                    RefreshTokens = rToken.Token,
                    ExpiresAt = token.ValidTo
                };
                return rTokenResponse;
            }
            //adding refresh tokens
            var refreshToken = new RefreshTokens()
            {
                JwtId = token.Id,
                IsRevoked = false,
                UserId = admin.Id,
                DateAdded = DateTime.UtcNow,
                DateExpire = DateTime.UtcNow.AddMonths(6),
                Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString()
            };
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            var response = new AuthResultVM()
            {
                Token = jwtToken,
                RefreshTokens = refreshToken.Token,
                ExpiresAt = token.ValidTo
            };

            return response;
        }
    }
}
