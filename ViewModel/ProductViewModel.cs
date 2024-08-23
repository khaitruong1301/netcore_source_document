using System;
using System.ComponentModel.DataAnnotations;

namespace api.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [MaxLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string ProductName { get; set; }

        public string Alias { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10000.")]
        [DataType(DataType.Currency, ErrorMessage = "Price must be a valid currency format.")]
        public decimal Price { get; set; }

        [Required]
        public bool Deleted { get; set; }


    }
}
