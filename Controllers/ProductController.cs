using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagementAPI.Data;
using ProductManagementAPI.Data.Interfaces;
using ProductManagementAPI.Data.Model;
using ProductManagementAPI.Data.ViewModel.ProductVM;
using ProductManagementAPI.Data.ViewModel.ResponseVM;
using ProductManagementAPI.Helper;
using System.Collections;
using System.Collections.Generic;

namespace ProductManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepo _productRepo;
        private readonly ProductMgtDbContext _dbContext;

        public ProductController(IProductRepo productRepo, ProductMgtDbContext _dbContext)
        {
            this._productRepo = productRepo;
            this._dbContext = _dbContext;
        }

        /// <summary>
        /// Adds a product
        /// </summary>
        /// <param name="productCreationVM"></param>
        /// <returns></returns>
        [HttpPost("AddProduct")]
        [AuthRoleValidationAttribute(AuthUserRoles.ADMIN1_USER, AuthUserRoles.ADMIN2_USER)]
        public async Task<IActionResult> PostProduct(ProductCreationVM product)
        {
            var resp = await _productRepo.AddProduct(product);
            return ResolveActionResult(resp);
        }

        [HttpPost("UpdateProductStatus")]
        [AuthRoleValidationAttribute(AuthUserRoles.ADMIN1_USER, AuthUserRoles.ADMIN2_USER)]
        public async Task<IActionResult> UpdateProductStatus(StatusUpdateVM entity)
        {
            var resp = await _productRepo.UpdateProductStatus(entity);
            return ResolveActionResult(resp);
        }


        /// <summary>
        /// Read all Products
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetProducts")]
        [AuthRoleValidationAttribute(AuthUserRoles.ADMIN1_USER, AuthUserRoles.ADMIN2_USER)]
        public async Task<IActionResult> GetProducts()
        {
            var resp = await _productRepo.GetProducts();
            return ResolveActionResult(resp);
        }

        /// <summary>
        /// Reads a product type by Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("GetProducts/{Id}")]
        [AuthRoleValidationAttribute(AuthUserRoles.ADMIN1_USER, AuthUserRoles.ADMIN2_USER)]
        public async Task<IActionResult> GetById(ProductReturnVM entity)
        {
            var resp = await _productRepo.GetById(entity);
            return ResolveActionResult(resp);
        }

        /// <summary>
        /// Updates a Product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut("UpdateProduct")]
        [AuthRoleValidationAttribute(AuthUserRoles.ADMIN1_USER, AuthUserRoles.ADMIN2_USER)]
        public async Task<IActionResult> UpdateProduct(StatusUpdateVM entity)
        {
            var resp = await _productRepo.UpdateProduct(entity);
            return ResolveActionResult(resp);
        }

        /// <summary>
        /// Deletes a product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteProduct")]
        [AuthRoleValidationAttribute(AuthUserRoles.ADMIN1_USER)]
        public async Task<IActionResult> DeleteProduct(StatusUpdateVM entity)
        {
            var resp = await _productRepo.DeleteProduct(entity);
            return ResolveActionResult(resp);
        }

        private bool ProductExists(int id)
        {
            return (_dbContext.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }

        protected ActionResult ResolveActionResult<T>(T response) where T : ApiBaseResponseVM
        {
            if (response.StatusCode == StatusCodes.Status200OK)
            {
                return Ok(response);
            }
            else if (response.StatusCode == StatusCodes.Status400BadRequest)
            {
                return BadRequest(response);
            }
            else if (response.StatusCode == StatusCodes.Status201Created)
            {
                return StatusCode(StatusCodes.Status201Created, response as T);
            }
            else if (response.StatusCode == StatusCodes.Status404NotFound)
            {
                return NotFound(response);
            }
            else if (response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                return Unauthorized(response);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

    }
}
