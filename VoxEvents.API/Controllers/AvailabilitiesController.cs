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
    [Route("api")]
    public class AvailabilitiesController : Controller
    {
        private ILogger<AvailabilitiesController> _logger;
        private readonly IVoxEventsRepository _repository;

        public AvailabilitiesController(ILogger<AvailabilitiesController> logger,
            IVoxEventsRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("events/{eventId}/availabilities")]
        public IActionResult GetVoxEventAllAvailabilities(int eventId)
        {
            try
            {
                if (!_repository.VoxEventExists(eventId))
                {
                    _logger.LogInformation($"Event with id {eventId} not found when getting all availabilities");
                    return NotFound();
                }

                var voxEventAvailabilityEntities = _repository.GetVoxEventAllAvailabilities(eventId);

                var results = Mapper.Map<IEnumerable<VoxEventAvailabilityDto>>(voxEventAvailabilityEntities);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception getting event id {eventId}'s availabilities", ex);
                return StatusCode(500, "A problem occurred handling your request");
            }
        }

        [HttpGet("members/{memberId}/availabilities")]
        public IActionResult GetMemberAllAvailabilities(int memberId)
        {
            try
            {
                if (!_repository.MemberExists(memberId))
                {
                    _logger.LogInformation($"Member with id {memberId} not found when accessing availabilities");
                    return NotFound();
                }

                var availabilitiesForMemberEntities = _repository.GetMemberAllAvailabilities(memberId);

                var results = Mapper.Map<IEnumerable<MemberAvailabilityDto>>(availabilitiesForMemberEntities);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception getting member id {memberId}'s availabilities", ex);
                return StatusCode(500, "A problem occurred handling your request");
            }
        }

        [HttpGet("members/{memberId}/availabilities/{voxEventId}", Name = "GetAvailability")]
        public IActionResult GetMemberAvailability(int memberId, int eventId)
        {
            try
            {
                if (!_repository.MemberExists(memberId))
                {
                    return NotFound();
                }

                var availabilityEntity = _repository.GetMemberAvailability(memberId, eventId);

                if (availabilityEntity == null)
                {
                    return NotFound();
                }

                var result = new MemberAvailabilityDto()
                {
                    VoxEventId = availabilityEntity.VoxEventId,
                    Available = availabilityEntity.Available
                };

                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogCritical($"Exception getting member id {memberId}'s availability for event id {eventId}", ex);
                return StatusCode(500, "A white person's problem occurred handling your request");
            }
        }

        [HttpPost("members/{memberId}/availabilities")]
        public IActionResult CreateMemberAvailability(int memberId,
            [FromBody] MemberAvailabilityCreateDto availability)
        {
            if (_repository.AvailabilityExists(memberId, availability.VoxEventId))
            {
                _logger.LogInformation($"Availability for member {memberId} event {availability.VoxEventId} already exists");
                return BadRequest();
            }

            if (availability == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!_repository.MemberExists(memberId))
            {
                return NotFound();
            }

            var finalMemberAvailability = Mapper.Map<Entities.Availability>(availability);

            _repository.AddMemberAvailability(memberId, finalMemberAvailability);

            if (!_repository.Save())
            {
                return StatusCode(500, "A problem occurred handling your request");
            }

            var newAvailability = Mapper.Map<Models.MemberAvailabilityDto>(finalMemberAvailability);

            return CreatedAtRoute("GetAvailability", new {
                memberId, voxEventId = newAvailability.VoxEventId }, newAvailability);
        }

        [HttpPatch("members/{memberId}/availabilities/{eventId}")]
        public IActionResult UpdateMemberAvailability(int memberId, int eventId, 
            [FromBody] JsonPatchDocument<MemberAvailabilityUpdateDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var member = VoxEventsDataStore.Current.Members.FirstOrDefault(m => m.Id == memberId);
            if (member == null)
            {
                return NotFound();
            }

            var availabilityFromStore = member.Availabilities.FirstOrDefault(a => a.VoxEventId == eventId);
            if (availabilityFromStore == null)
            {
                return NotFound();
            }

            var availabilityToPatch = new MemberAvailabilityUpdateDto()
            {
                Available = availabilityFromStore.Available
            };

            patchDocument.ApplyTo(availabilityToPatch, ModelState);

            TryValidateModel(availabilityToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            availabilityFromStore.Available = availabilityToPatch.Available;

            return NoContent();
        }

        //[HttpGet("events/{eventId}/availabilities/{memberId}")]
        //public IActionResult GetAvailability(int eventId, int memberId)
        //{
        //    var eventToCheck = EventsDataStore.Current.Events.FirstOrDefault(e => e.Id == eventId);
        //    if (eventToCheck == null)
        //    {
        //        return NotFound();
        //    }

        //    var memberToCheck = EventsDataStore.Current.Members.FirstOrDefault(m => m.Id == memberId);
        //    if (memberToCheck == null)
        //    {
        //        return NotFound();
        //    }
        //}
    }
}
