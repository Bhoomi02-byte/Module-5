using System.ComponentModel.DataAnnotations;

namespace Module_5.DTO
{
    public class CategoryDto
    {
        [Required(ErrorMessage = "CategoryName is required.")]
        public string CategoryName { get; set; }

        public string Description { get; set; }


    }
}
