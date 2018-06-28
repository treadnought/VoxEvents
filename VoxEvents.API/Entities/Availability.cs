using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxEvents.API.Entities
{
    public class Availability
    {
        public int MemberId { get; set; }
        public int VoxEventId { get; set; }

        [Required]
        public bool Available { get; set; }

        public Member Member { get; set; }
        public VoxEvent VoxEvent { get; set; }
    }
}
