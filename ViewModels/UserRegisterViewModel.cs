using System;
using System.ComponentModel.DataAnnotations;

namespace api.ViewModels;

public class UserRegisterViewModel
{
    [Required]
    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    public string FirstName { get; set; } = "";

    [Required]
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    public string LastName { get; set; } = "";

    [Required]
    [EmailAddress]
    [StringLength(256, ErrorMessage = "Email cannot exceed 256 characters")]
    public string Email { get; set; } = "";
    
    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    [MaxLength(128, ErrorMessage = "Password cannot exceed 128 characters")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", 
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character (@$!%*?&)")]
    public string Password { get; set; } = "";
    
    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = "";

    [Required]
    [RegularExpression("^(Admin|User)$", ErrorMessage = "Role must be either 'Admin' or 'User'")]
    public string Role { get; set; } = "User";
}
