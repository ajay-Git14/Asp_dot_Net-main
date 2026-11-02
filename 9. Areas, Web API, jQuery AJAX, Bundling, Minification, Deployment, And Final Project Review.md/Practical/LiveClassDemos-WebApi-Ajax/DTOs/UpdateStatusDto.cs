using System.ComponentModel.DataAnnotations;

namespace LiveClassDemos.DTOs;

public class UpdateStatusDto
{
    [Required]
    public string NewStatus { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Remarks { get; set; }
}
