using AutoMapper;
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
            try
            {
                var memberEntities = _repository.GetMembers();

                var results = Mapper.Map<IEnumerable<MemberNoAvailabilitiesDto>>(memberEntities);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception getting members", ex);
                return StatusCode(500, "A problem occurred handling your request");
            }
        }

        [HttpGet("{id}", Name = "GetMember")]
        public IActionResult GetMember(int id, bool includeAvailabilities = false)
        {
            try
            {
                var memberEntity = _repository.GetMember(id, includeAvailabilities);

                if (memberEntity == null)
                {
                    return NotFound();
                }

                if (includeAvailabilities)
                {
                    var memberResult = Mapper.Map<MemberDto>(memberEntity);

                    return Ok(memberResult);
                }

                var memberNoAvailabilitiesResult = Mapper.Map<MemberNoAvailabilitiesDto>(memberEntity);

                return Ok(memberNoAvailabilitiesResult);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception getting member id {id}", ex);
                return StatusCode(500, "A problem occurred handling your request");
            }
        }

        [HttpPost]
        public IActionResult CreateMember([FromBody] MemberCreateDto member)
        {
            try
            {
                if (member == null)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var memberToAdd = Mapper.Map<Entities.Member>(member);

                _repository.AddMember(memberToAdd);

                if (!_repository.Save())
                {
                    return StatusCode(500, "A problem occurred handling your request");
                }

                var newMember = Mapper.Map<MemberDto>(memberToAdd);

                return CreatedAtRoute("GetMember", new
                { id = newMember.Id }, newMember);

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception adding member", ex);
                return StatusCode(500, "A problem occurred handling your request");
            }
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

            var memberEntity = _repository.GetMember(id);
            if (memberEntity == null)
            {
                return NotFound();
            }

            var memberToPatch = Mapper.Map<MemberUpdateDto>(memberEntity);

            patchDocument.ApplyTo(memberToPatch, ModelState);

            TryValidateModel(memberToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(memberToPatch, memberEntity);

            if (!_repository.Save())
            {
                return StatusCode(500, "A problem occurred handling your request");
            }

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
