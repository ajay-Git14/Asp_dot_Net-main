using System.ComponentModel.DataAnnotations;

namespace LiveClassDemos.Models
{
    public class Student
    {
        [Required(ErrorMessage = "Name is required field")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required field")]
        [EmailAddress(ErrorMessage = "Email should valid")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Course is required")]
        public string Course { get; set; }
        [Range(18, 60, ErrorMessage = "Please enter valid age from 18 to 60")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Image Path is creating")]
        public string ImagePath { get; set; } = "testing.md";
    }
}
