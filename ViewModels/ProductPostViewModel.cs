using System.ComponentModel.DataAnnotations;

namespace api.ViewModels;

public class ProductPostViewModel
{
    [Required(ErrorMessage = "Artikelnummer måste anges.")]
    [MaxLength(8, ErrorMessage = "Artikelnummer får bara vara 8 tecken.")]
    [MinLength(8, ErrorMessage = "Artikelnummer måste vara 8 tecken.")]
    public required string ItemNumber { get; set; }

    [Required]
    public required string Category { get; set; }

    [Required]
    public required string Brand { get; set; }

    [Required]
    public required string Name { get; set; }

    [MaxLength(256)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Pris måste anges.")]
    public double Price { get; set; }
}
