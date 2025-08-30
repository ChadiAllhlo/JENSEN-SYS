using System.ComponentModel.DataAnnotations;

namespace api.Models;

public class Todo
{
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public bool IsCompleted { get; set; } = false;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? CompletedAt { get; set; }
    
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    public User User { get; set; } = null!;
}
