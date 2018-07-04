using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxEvents.API.Models
{
    public class VoxEventUpdateDto
    {
        [Required(ErrorMessage = "Event name is required")]
        [MaxLength(50), MinLength(2)]
        public string EventName { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime EventDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime RehearsalTime { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime PerformanceTime { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int VenueId { get; set; }
    }
}
