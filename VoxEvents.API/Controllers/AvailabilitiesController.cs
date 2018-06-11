using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxEvents.API.Controllers
{
    [Route("api")]
    public class AvailabilitiesController : Controller
    {
        [HttpGet("events/{eventId}/availabilities")]
        public IActionResult GetEventAllAvailabilities(int eventId)
        {
            var eventToCheck = EventsDataStore.Current.Events.FirstOrDefault(e => e.Id == eventId);

            if (eventToCheck == null)
            {
                return NotFound();
            }

            return Ok(eventToCheck.Availabilities);
        }

        [HttpGet("members/{memberId}/availabilities")]
        public IActionResult GetMemberAllAvailabilities(int memberId)
        {
            var member = EventsDataStore.Current.Members.FirstOrDefault(m => m.Id == memberId);

            if (member == null)
            {
                return NotFound();
            }

            return Ok(member.Availabilities);
        }
    }
}
