using System.ComponentModel.DataAnnotations;

namespace ProductManagementAPI.Data.ViewModel
{
    public class LoginVM
    {
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
