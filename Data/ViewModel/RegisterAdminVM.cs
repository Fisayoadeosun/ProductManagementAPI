using System.ComponentModel.DataAnnotations;

namespace ProductManagementAPI.Data.ViewModel
{
    public class RegisterAdminVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}

