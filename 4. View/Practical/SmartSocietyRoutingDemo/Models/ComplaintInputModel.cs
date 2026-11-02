using System.ComponentModel.DataAnnotations;

namespace SmartSocietyRoutingDemo.Models;

public class ComplaintInputModel
{
    [Required]
    [StringLength(80, MinimumLength = 5)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^[A-Za-z]+$", ErrorMessage = "Block should contain letters only.")]
    public string Block { get; set; } = string.Empty;
}
