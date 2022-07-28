using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProductManagementAPI.Data.Model
{
    public class Admin : IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserRole { get; set; }
        public bool Login { get; set; }
        public bool Logout { get; set; }
    }
}
