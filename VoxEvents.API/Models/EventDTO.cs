﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxEvents.API.Models
{
    public class EventDTO
    {
        public int Id { get; set; }
        public string EventName { get; set; }
        public string EventVenue { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime RehearsalTime { get; set; }
        public DateTime PerformanceTime { get; set; }
    }
}