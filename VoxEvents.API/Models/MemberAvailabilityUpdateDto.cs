using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxEvents.API.Models
{
    public class MemberAvailabilityUpdateDto
    {
        // only the Available field should ever be updated
        [Required]
        public bool Available { get; set; }
    }
}
