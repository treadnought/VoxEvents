using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxEvents.API.Models
{
    public class MemberCreateDto
    {
        [Required]
        [MaxLength(50), MinLength(2)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50), MinLength(2)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        public Parts Part { get; set; }
    }
}
