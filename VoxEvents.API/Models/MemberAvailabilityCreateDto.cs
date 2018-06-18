using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxEvents.API.Models
{
    public class MemberAvailabilityCreateDto
    {
        [Required]
        public int MemberId { get; set; }
        [Required]
        public int EventId { get; set; }
        [Required]
        public bool Available { get; set; }
    }
}
