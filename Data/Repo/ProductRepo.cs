using Microsoft.EntityFrameworkCore;
using ProductManagementAPI.Data.Interfaces;
using ProductManagementAPI.Data.Model;
using ProductManagementAPI.Data.ViewModel.ProductVM;
using ProductManagementAPI.Data.ViewModel.ResponseVM;

namespace ProductManagementAPI.Data.Repo
{
    public class ProductRepo : IProductRepo
    {
        private readonly ProductMgtDbContext _dbContext;

        public ProductRepo(ProductMgtDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<ApiBaseResponseVM> AddProduct(ProductCreationVM ProductReturnVM)
        {
            // initiate db model
            var product = new Product();
            product.ProductName = ProductReturnVM.ProductName;
            product.Price = ProductReturnVM.Price;
            product.DateCreated = DateTime.Now;

            try
            {
                // add data into db table
                await _dbContext.Products.AddAsync(product);
                await _dbContext.SaveChangesAsync();

                // initiate response
                var responseVm = new ProductReturnVM() { ProductId = product.ProductId, ProductName = product.ProductName, Price = product.Price };

                // retrun resp
                return new ApiBaseDataResponseVM<ProductReturnVM> { Data = responseVm, Message = "Success", Status = true };
            }
            catch (Exception)
            {
                //return exception response
                return new ApiBaseResponseVM { Message = "Error occured", Status = false };
            }
        }



        public async Task<ApiBaseResponseVM> DeleteProduct(StatusUpdateVM entity)
        {
            // checks if products exist in db
            if (_dbContext.Products == null)
            {
                return new ApiBaseResponseVM { Message = "Error occured", Status = false };
            }
            var product = await _dbContext.Products.FindAsync(entity);
            if (product == null)
            {
                return new ApiBaseResponseVM { Message = "Error occured", Status = false };
            }
            //remove or delete data from db and save changes
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            return new ApiBaseDataResponseVM<Product> { Data = product, Message = "Success", Status = true };
        }



        public async Task<ApiBaseResponseVM> GetById(ProductReturnVM productReturnVM)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync();
            //return response
            return new ApiBaseDataResponseVM<Product> { Data = product, Message = "Success", Status = true };
        }



        public async Task<ApiBaseResponseVM> GetProducts()
        {
            var products = await _dbContext.Products.ToListAsync();
            // retrun resp
            return new ApiBaseDataResponseVM<List<Product>> { Data = products, Message = "Success", Status = true };
        }



        public async Task<ApiBaseResponseVM> UpdateProduct(StatusUpdateVM entity)
        {
            var product = await _dbContext.Products.FindAsync(entity.ProductId);
            if(product == null)
            {
                return new ApiBaseResponseVM { Message = "Not found", Status = false, StatusCode = StatusCodes.Status404NotFound };
            }
            product.Status = entity.Status;

            //save update changes
            await _dbContext.SaveChangesAsync();
            //return response
            return new ApiBaseDataResponseVM<List<ProductUpdateVM>> { Message = "Success", Status = true };
        }




        public async Task<ApiBaseResponseVM> UpdateProductStatus(StatusUpdateVM entity)
        {
            var product = await _dbContext.Products.FindAsync(entity.ProductId);

            if(product == null)
            {
                return new ApiBaseResponseVM { Message = "Not found", Status = false, StatusCode = StatusCodes.Status404NotFound };
            }

            product.Status = entity.Status;

            // save update changes
            await _dbContext.SaveChangesAsync();

            // retrun resp
            return new ApiBaseDataResponseVM<ProductReturnVM> { Message = "Success", Status = true };
        }
    }
}
