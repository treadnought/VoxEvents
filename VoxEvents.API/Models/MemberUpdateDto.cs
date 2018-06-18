using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxEvents.API.Models
{
    public class MemberUpdateDto
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
        public Parts Part { get; set; }
    }
}
