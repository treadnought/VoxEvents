using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxEvents.API.Controllers
{
    [Route("api/events")]
    public class EventsController : Controller
    {
        [HttpGet]
        public IActionResult GetEvents()
        {
            return Ok(EventsDataStore.Current.Events);
        }

        [HttpGet("{id}")]
        public IActionResult GetEvent(int id)
        {
            var eventToReturn = EventsDataStore.Current.Events.FirstOrDefault(e => e.Id == id);
            if (eventToReturn == null)
            {
                return NotFound();
            }
            return Ok(eventToReturn);
        }
    }
}
