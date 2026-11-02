using System.ComponentModel.DataAnnotations;

namespace LiveClassDemos.DTOs;

public class CreateComplaintDto
{
    [Required, StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(1000, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Category { get; set; } = string.Empty;

    [Required]
    public string Priority { get; set; } = string.Empty;
}
