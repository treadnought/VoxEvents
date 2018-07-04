using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxEvents.API.Entities
{
    public class VoxEvent
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Event name is required")]
        [MaxLength(50), MinLength(2)]
        public string EventName { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EventDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime RehearsalTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime PerformanceTime { get; set; }

        public ICollection<Availability> Availabilities { get; set; } 
            = new List<Availability>();

        [Required]
        [Range(0, int.MaxValue)]
        public int VenueId { get; set; }
        public Venue Venue { get; set; }
    }
}
