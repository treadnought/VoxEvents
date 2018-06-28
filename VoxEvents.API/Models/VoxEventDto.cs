using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxEvents.API.Models
{
    public class VoxEventDto
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime RehearsalTime { get; set; }
        public DateTime PerformanceTime { get; set; }

        public int NumberOfAvailabilites { get
            {
                return Availabilities.Count();
            }
        }

        public ICollection<MemberAvailabilityDto> Availabilities { get; set; }
            = new List<MemberAvailabilityDto>();
        public int VenueId { get; set; }
        public VenueDto Venue { get; set; }
    }
}
