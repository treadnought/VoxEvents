using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxEvents.API.Models;

namespace VoxEvents.API.Controllers
{
    [Route("api/events")]
    public class VoxEventsController : Controller
    {
        [HttpGet]
        public IActionResult GetEvents()
        {
            return Ok(VoxEventsDataStore.Current.Events);
        }

        [HttpGet("{id}")]
        public IActionResult GetEvent(int id)
        {
            var eventToReturn = VoxEventsDataStore.Current.Events.FirstOrDefault(e => e.Id == id);
            if (eventToReturn == null)
            {
                return NotFound();
            }
            return Ok(eventToReturn);
        }

        //[HttpPost]
        //public IActionResult CreateEvent([FromBody] VoxEventCreateDto choirEvent)
        //{
        //    if (choirEvent == null)
        //    {
        //        return BadRequest();
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }


        //}
    }
}
