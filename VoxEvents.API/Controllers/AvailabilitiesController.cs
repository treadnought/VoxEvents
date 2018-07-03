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
        public IActionResult GetMemberAvailability(int memberId, int voxEventId)
        {
            try
            {
                if (!_repository.MemberExists(memberId))
                {
                    return NotFound();
                }

                var availabilityEntity = _repository.GetMemberAvailability(memberId, voxEventId);

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
                _logger.LogCritical($"Exception getting member id {memberId}'s availability for event id {voxEventId}", ex);
                return StatusCode(500, "A white person's problem occurred handling your request");
            }
        }

        [HttpPost("members/{memberId}/availabilities")]
        public IActionResult CreateMemberAvailability(int memberId,
            [FromBody] MemberAvailabilityCreateDto availability)
        {
            try
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

                var newAvailability = Mapper.Map<MemberAvailabilityDto>(finalMemberAvailability);

                return CreatedAtRoute("GetAvailability", new
                {
                    memberId,
                    voxEventId = newAvailability.VoxEventId
                }, newAvailability);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception adding member id {memberId}'s availability", ex);
                return StatusCode(500, "A white person's problem occurred handling your request");
            }
        }

        [HttpPut("members/{memberId}/availabilities/{voxEventId}")]
        public IActionResult UpdateMemberAvailability(int memberId, int voxEventId, 
            [FromBody] MemberAvailabilityUpdateDto availability)
        {
            try
            {
                if (availability == null)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_repository.MemberExists(memberId))
                {
                    return NotFound();
                }

                var memberAvailabilityEntity = _repository.GetMemberAvailability(memberId, voxEventId);

                if (memberAvailabilityEntity == null)
                {
                    return NotFound();
                }

                // override values in entity with those from passed in object. EF will mark entity as modified
                Mapper.Map(availability, memberAvailabilityEntity);

                if (!_repository.Save())
                {
                    return StatusCode(500, "A problem occurred handling your request");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception updating member id {memberId}'s availability for event id {voxEventId}", ex);
                return StatusCode(500, "A white person's problem occurred handling your request");
            }
        }

        [HttpPatch("members/{memberId}/availabilities/{voxEventId}")]
        public IActionResult UpdateMemberAvailability(int memberId, int voxEventId, 
            [FromBody] JsonPatchDocument<MemberAvailabilityUpdateDto> patchDocument)
        {
            try
            {
                if (patchDocument == null)
                {
                    return BadRequest();
                }

                var memberAvailabilityEntity = _repository.GetMemberAvailability(memberId, voxEventId);
                if (memberAvailabilityEntity == null)
                {
                    return NotFound();
                }

                var availabilityToPatch = Mapper.Map<MemberAvailabilityUpdateDto>(memberAvailabilityEntity);

                patchDocument.ApplyTo(availabilityToPatch, ModelState);

                TryValidateModel(availabilityToPatch);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Mapper.Map(availabilityToPatch, memberAvailabilityEntity);

                if (!_repository.Save())
                {
                    return StatusCode(500, "A problem occurred handling your request");
                }

                return NoContent();

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception patching member id {memberId}'s availability for event id {voxEventId}", ex);
                return StatusCode(500, "A problem occurred handling your request");
            }
        }
    }
}
