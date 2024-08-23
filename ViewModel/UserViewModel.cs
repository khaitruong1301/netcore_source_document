using System.ComponentModel.DataAnnotations;

namespace  api.Models.ViewModel;

public class UserViewModel
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 50 characters")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Full name is required")]
    [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Confirm Password is required")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = null!;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; } = null!;

    [Phone(ErrorMessage = "Invalid phone number")]
    public string? Phone { get; set; }
}



