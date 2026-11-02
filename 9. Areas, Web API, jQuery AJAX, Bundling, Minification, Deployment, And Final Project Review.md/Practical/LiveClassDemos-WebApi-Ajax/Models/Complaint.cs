using System.ComponentModel.DataAnnotations;

namespace LiveClassDemos.Models;

public class Complaint
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Category { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Priority { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string Status { get; set; } = "New";

    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public bool IsArchived { get; set; }
}
