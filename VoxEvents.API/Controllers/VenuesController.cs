using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxEvents.API.Controllers
{
    [Route("api/venues")]
    public class VenuesController : Controller
    {
        [HttpGet]
        public IActionResult GetVenues()
        {
            return Ok(EventsDataStore.Current.Venues);
        }

        [HttpGet("{id}")]
        public IActionResult GetVenue(int id)
        {
            var venueToReturn = EventsDataStore.Current.Venues.FirstOrDefault(v => v.Id == id);
            if (venueToReturn == null)
            {
                return NotFound();
            }
            return Ok(venueToReturn);
        }
    }
}
