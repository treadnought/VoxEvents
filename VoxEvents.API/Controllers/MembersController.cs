using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxEvents.API.Controllers
{
    [Route("api/members")]
    public class MembersController : Controller
    {
        [HttpGet]
        public IActionResult GetMembers()
        {
            return Ok(EventsDataStore.Current.Members);
        }

        [HttpGet("{id}")]
        public IActionResult GetMember(int id)
        {
            var memberToReturn = EventsDataStore.Current.Members.FirstOrDefault(m => m.Id == id);

            if (memberToReturn == null)
            {
                return NotFound();
            }

            return Ok(memberToReturn);
        }
    }
}
