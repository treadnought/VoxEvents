using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxEvents.API.Models
{
    public class Availability
    {
        public int EventId { get; set; }
        public int MemberId { get; set; }
        public bool Available { get; set; }
    }
}
