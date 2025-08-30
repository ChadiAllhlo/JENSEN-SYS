using System.ComponentModel.DataAnnotations;

namespace api.ViewModels;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    [StringLength(256, ErrorMessage = "Email cannot exceed 256 characters")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(128, ErrorMessage = "Password cannot exceed 128 characters")]
    public string Password { get; set; } = string.Empty;
}
