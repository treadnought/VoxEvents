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
        public IActionResult GetEventAllAvailabilities(int eventId)
        {
            var eventToCheck = VoxEventsDataStore.Current.Events.FirstOrDefault(e => e.Id == eventId);

            if (eventToCheck == null)
            {
                _logger.LogInformation($"Event with id {eventId} not found when getting all availabilities");
                return NotFound();
            }

            return Ok(eventToCheck.Availabilities);
        }

        [HttpGet("members/{memberId}/availabilities")]
        public IActionResult GetMemberAllAvailabilities(int memberId)
        {
            //var member = VoxEventsDataStore.Current.Members.FirstOrDefault(m => m.Id == memberId);

            try
            {
                if (!_repository.MemberExists(memberId))
                {
                    _logger.LogInformation($"Member with id {memberId} not found when accessing availabilities");
                    return NotFound();
                }

                var availabilitesForMemberEntities = _repository.GetMemberAllAvailabilities(memberId);

                var results = new List<MemberAvailabilityDto>();

                foreach (var availabilityEntity in availabilitesForMemberEntities)
                {
                    results.Add(new MemberAvailabilityDto
                    {
                        VoxEventId = availabilityEntity.VoxEventId,
                        Available = availabilityEntity.Available
                    });
                }

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception getting member id {memberId}'s availabilities", ex);
                return StatusCode(500, "A problem occurred handling your request");
            }
        }

        [HttpGet("members/{memberId}/availabilities/{eventId}", Name = "GetAvailability")]
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
                //var member = VoxEventsDataStore.Current.Members.FirstOrDefault(m => m.Id == memberId);
                //if (member == null)
                //{
                //    _logger.LogInformation($"Member with id {memberId} not found");
                //    return NotFound();
                //}

                //var availability = member.Availabilities.FirstOrDefault(e => e.VoxEventId == eventId);
                //if (availability == null)
                //{
                //    return NotFound();
                //}

                //return Ok(availability);
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
            if (availability == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var member = VoxEventsDataStore.Current.Members.FirstOrDefault(m => m.Id == memberId);
            if (member == null)
            {
                return NotFound();
            }

            var finalMemberAvailability = new MemberAvailabilityDto()
            {
                VoxEventId = availability.VoxEventId,
                Available = availability.Available
            };

            member.Availabilities.Add(finalMemberAvailability);

            return CreatedAtRoute("GetAvailability", new
                { memberId, eventId = availability.VoxEventId }, 
                finalMemberAvailability);
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
