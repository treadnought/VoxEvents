﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxEvents.API.Models
{
    public class VenueDto
    {
        public int Id { get; set; }
        public string VenueName { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public bool HasPiano { get; set; }

        public int NumberOfEvents { get
            {
                return VoxEvents.Count();
            }
        }

        public ICollection<VoxEventDto> VoxEvents { get; set; } = new List<VoxEventDto>();
    }
}
