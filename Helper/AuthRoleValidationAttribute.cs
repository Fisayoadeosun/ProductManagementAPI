using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using ProductManagementAPI.Data.ViewModel.ResponseVM;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProductManagementAPI.Helper
{
    public class AuthRoleValidationAttribute : ActionFilterAttribute
    {
        private readonly string[] _roles;

        public AuthRoleValidationAttribute()
        {
        }

        public AuthRoleValidationAttribute(params string[] roles)
        {
            this._roles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var hasAuth = context.HttpContext.Request.Headers.TryGetValue(HeaderNames.Authorization, out var authValue);
            if (hasAuth)
            {
                var tokenString = authValue.ToString().Split(" ", StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
                var token = new JwtSecurityToken(tokenString);
                if (token != null)
                { 
                    if (DateTime.Compare(DateTime.UtcNow, token.ValidTo) <= 0)
                    {
                        if(_roles != null && _roles.Any())
                        {
                            var authRole = 0;
                            var tokenRole = token.Claims.FirstOrDefault(v => v.Type == ClaimTypes.Role);
                            if(tokenRole != null)
                            {
                                foreach (var role in _roles)
                                    if (role == tokenRole.Value)
                                        authRole = authRole + 1;


                                if(authRole <= 0)
                                {
                                    context.Result = new UnauthorizedObjectResult(
                                       new ApiBaseResponseVM
                                       {
                                           Message = "UNAUTHORIZED",
                                           Status = false,
                                           StatusCode = StatusCodes.Status401Unauthorized
                                       });
                                }

                                // proceed
                                base.OnActionExecuting(context);
                            }
                            else
                            {
                                context.Result = new UnauthorizedObjectResult(
                                   new ApiBaseResponseVM
                                   {
                                       Message = "UNAUTHORIZED",
                                       Status = false,
                                       StatusCode = StatusCodes.Status401Unauthorized
                                   });
                            }
                        }
                    }
                    else
                    {
                        context.Result = new UnauthorizedObjectResult(
                            new ApiBaseResponseVM
                            {
                                Message = "UNAUTHORIZED",
                                Status = false,
                                StatusCode = StatusCodes.Status401Unauthorized
                            });
                    }
                }
                else
                {
                    context.Result = new UnauthorizedObjectResult(
                            new ApiBaseResponseVM
                            {
                                Message = "UNAUTHORIZED",
                                Status = false,
                                StatusCode = StatusCodes.Status401Unauthorized
                            });
                }

            }
            else
            {
                context.Result = new UnauthorizedObjectResult(
                    new ApiBaseResponseVM
                    {
                        Message = "UNAUTHORIZED",
                        Status = false,
                        StatusCode = StatusCodes.Status401Unauthorized
                    });

            }  
        }
    }
}
