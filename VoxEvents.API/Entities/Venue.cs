﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxEvents.API.Entities
{
    public class Venue
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string VenueName { get; set; }
        public string Address { get; set; }
        public bool HasPiano { get; set; }
    }
}
