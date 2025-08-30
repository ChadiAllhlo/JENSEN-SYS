using System.ComponentModel.DataAnnotations;

namespace api.ViewModels;

public class TodoCreateViewModel
{
    [Required]
    [MaxLength(128)]
    public required string Title { get; set; }

    [MaxLength(512)]
    public string? Description { get; set; }
}

