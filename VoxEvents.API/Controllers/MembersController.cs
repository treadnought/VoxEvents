using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxEvents.API.Models;

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

        [HttpGet("{id}", Name = "GetMember")]
        public IActionResult GetMember(int id)
        {
            var memberToReturn = EventsDataStore.Current.Members.FirstOrDefault(m => m.Id == id);

            if (memberToReturn == null)
            {
                return NotFound();
            }

            return Ok(memberToReturn);
        }

        [HttpPost]
        public IActionResult CreateMember([FromBody] MemberCreateDto member)
        {
            if (member == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var maxMemberId = EventsDataStore.Current.Members.Max(m => m.Id);

            var memberToAdd = new MemberDto()
            {
                Id = ++maxMemberId,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Email = member.Email,
                Phone = member.Phone,
                Part = member.Part
            };

            EventsDataStore.Current.Members.Add(memberToAdd);

            return CreatedAtRoute("GetMember", new
            { id = memberToAdd.Id }, memberToAdd);
        }
    }
}
