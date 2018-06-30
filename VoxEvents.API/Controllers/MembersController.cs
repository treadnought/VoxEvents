using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxEvents.API.Models;
using VoxEvents.API.Services;

namespace VoxEvents.API.Controllers
{
    [Route("api/members")]
    public class MembersController : Controller
    {
        private readonly IVoxEventsRepository _repository;
        private readonly ILogger<MembersController> _logger;
        private readonly IMailService _mailService;

        public MembersController(IVoxEventsRepository repository, ILogger<MembersController> logger, 
            IMailService mailService)
        {
            _repository = repository;
            _logger = logger;
            _mailService = mailService;
        }

        [HttpGet]
        public IActionResult GetMembers()
        {
            var memberEntities = _repository.GetMembers();

            var results = new List<MemberNoAvailabilitiesDto>();

            foreach (var memberEntity in memberEntities)
            {
                results.Add(new MemberNoAvailabilitiesDto
                {
                    Id = memberEntity.Id,
                    FirstName = memberEntity.FirstName,
                    LastName = memberEntity.LastName,
                    Email = memberEntity.Email,
                    Phone = memberEntity.Phone,
                    Part = memberEntity.Part
                });
            }

            return Ok(results);
            //return Ok(VoxEventsDataStore.Current.Members);
        }

        [HttpGet("{id}", Name = "GetMember")]
        public IActionResult GetMember(int id)
        {
            var memberToReturn = VoxEventsDataStore.Current.Members.FirstOrDefault(m => m.Id == id);

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
                return BadRequest(ModelState);
            }

            var maxMemberId = VoxEventsDataStore.Current.Members.Max(m => m.Id);

            var memberToAdd = new MemberDto()
            {
                Id = ++maxMemberId,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Email = member.Email,
                Phone = member.Phone,
                Part = member.Part
            };

            VoxEventsDataStore.Current.Members.Add(memberToAdd);

            return CreatedAtRoute("GetMember", new
            { id = memberToAdd.Id }, memberToAdd);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMember(int id, [FromBody] MemberUpdateDto member)
        {
            if (member == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var memberFromStore = VoxEventsDataStore.Current.Members.FirstOrDefault(m => m.Id == id);

            if (memberFromStore == null)
            {
                return NotFound();
            }

            memberFromStore.FirstName = member.FirstName;
            memberFromStore.LastName = member.LastName;
            memberFromStore.Email = member.Email;
            memberFromStore.Phone = member.Phone;
            memberFromStore.Part = member.Part;

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PatchMember(int id, [FromBody] JsonPatchDocument<MemberUpdateDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var memberFromStore = VoxEventsDataStore.Current.Members.FirstOrDefault(m => m.Id == id);

            if (memberFromStore == null)
            {
                return NotFound();
            }

            var memberToPatch = new MemberUpdateDto()
            {
                FirstName = memberFromStore.FirstName,
                LastName = memberFromStore.LastName,
                Email = memberFromStore.Email,
                Phone = memberFromStore.Phone,
                Part = memberFromStore.Part
            };

            patchDocument.ApplyTo(memberToPatch, ModelState);

            TryValidateModel(memberToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            memberFromStore.FirstName = memberToPatch.FirstName;
            memberFromStore.LastName = memberToPatch.LastName;
            memberFromStore.Email = memberToPatch.Email;
            memberFromStore.Phone = memberToPatch.Phone;
            memberFromStore.Part = memberToPatch.Part;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMember(int id)
        {
            var memberFromStore = VoxEventsDataStore.Current.Members.FirstOrDefault(m => m.Id == id);

            if (memberFromStore == null)
            {
                return NotFound();
            }

            VoxEventsDataStore.Current.Members.Remove(memberFromStore);

            _mailService.Send("Member Deleted", $"Member {memberFromStore.FirstName} {memberFromStore.LastName} with id {id} deleted.");

            return NoContent();
        }
    }
}
