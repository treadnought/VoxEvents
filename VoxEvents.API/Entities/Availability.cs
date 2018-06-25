using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxEvents.API.Entities
{
    public class Availability
    {
        public int MemberId { get; set; }
        public int EventId { get; set; }
        public bool Available { get; set; }

        public Member Member { get; set; }
    }
}
