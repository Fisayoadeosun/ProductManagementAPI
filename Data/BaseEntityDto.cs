using System.ComponentModel.DataAnnotations;

namespace ProductManagementAPI.Data
{
    public class BaseEntityDto
    {
        [Required]
        public string Name { get; set; }


        [Required]
        public string Description { get; set; }
    }
}
