using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductManagementAPI.Data.Model;
using ProductManagementAPI.Data.ViewModel;
using ProductManagementAPI.Data.ViewModel.ProductVM;
using ProductManagementAPI.Data.ViewModel.ResponseVM;

namespace ProductManagementAPI.Data
{
    public class ProductMgtDbContext : IdentityDbContext<Admin>
    {
        public ProductMgtDbContext(DbContextOptions<ProductMgtDbContext> options) : base(options)
        {

        }

        public DbSet<RefreshTokens> RefreshTokens { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
