using System.ComponentModel.DataAnnotations;

namespace api.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [MaxLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string CategoryName { get; set; }
        
        public string Alias { get; set; }

        // Dạng DateTime cho xử lý bên trong server
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        [Required]
        public bool Deleted { get; set; }

        // Dạng string nhận từ client (ddMMyyyy)
        public string CreatedAtString { get; set; }
        public string UpdatedAtString { get; set; }
    }
}
