using ProductManagementAPI.Data.Model;
using ProductManagementAPI.Data.ViewModel.ProductVM;
using ProductManagementAPI.Data.ViewModel.ResponseVM;

namespace ProductManagementAPI.Data.Interfaces
{
    public interface IProductRepo
    {
        public Task<ApiBaseResponseVM> AddProduct(ProductCreationVM ProductReturnVM);
        public Task<ApiBaseResponseVM> UpdateProductStatus(StatusUpdateVM entity);
        public Task<ApiBaseResponseVM> GetProducts();
        public Task<ApiBaseResponseVM> GetById(ProductReturnVM productReturnVM);
        public Task<ApiBaseResponseVM> UpdateProduct(StatusUpdateVM entity);
        public Task<ApiBaseResponseVM> DeleteProduct(StatusUpdateVM entity);
    }
}
