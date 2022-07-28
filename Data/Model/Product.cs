namespace ProductManagementAPI.Data.Model
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool Status { get; set; } = true;
        public DateTime? DateDeleted { get; set; }

    }
}
