using System.ComponentModel.DataAnnotations;

namespace ActionFlowDemo.Models;

public sealed class CreateProductViewModel
{
    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(80, MinimumLength = 2)]
    [Display(Name = "Product name")]
    public string Name { get; set; } = string.Empty;

    [Range(1, 1_000_000, ErrorMessage = "Price must be between 1 and 1,000,000.")]
    public decimal Price { get; set; }

    [Display(Name = "Show as featured")]
    public bool IsFeatured { get; set; }
}
