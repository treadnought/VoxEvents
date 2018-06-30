using System.ComponentModel.DataAnnotations;

namespace VoxEvents.API.Models
{
    public class MemberCreateDto
    {
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50), MinLength(2)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50), MinLength(2)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Email address is invalid")]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Part { get; set; }
    }
}
