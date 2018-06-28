using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxEvents.API.Models
{
    public class MemberAvailabilityDto
    {
        public int MemberId { get; set; }
        public int VoxEventId { get; set; }
        public bool Available { get; set; }
    }
}
