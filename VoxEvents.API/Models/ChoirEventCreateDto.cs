﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxEvents.API.Models
{
    public class ChoirEventCreateDto
    {
        [Required(ErrorMessage = "Event name is required")]
        [MaxLength(50), MinLength(2)]
        public string EventName { get; set; }
        [Required(ErrorMessage = "Venue name is required")]
        [MaxLength(50), MinLength(2)]
        public string EventVenue { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime EventDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime RehearsalTime { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime PerformanceTime { get; set; }
    }
}
