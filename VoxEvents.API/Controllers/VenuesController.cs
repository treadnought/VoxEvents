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
    [Route("api/venues")]
    public class VenuesController : Controller
    {
        private readonly ILogger<VenuesController> _logger;
        private readonly IVoxEventsRepository _repository;
        private readonly IMailService _mailService;

        public VenuesController(ILogger<VenuesController> logger, 
            IVoxEventsRepository repository,
            IMailService mailService)
        {
            _logger = logger;
            _repository = repository;
            _mailService = mailService;
        }

        [HttpGet]
        public IActionResult GetVenues()
        {
            try
            {
                var venuesEntities = _repository.GetVenues();
                var results = Mapper.Map<IEnumerable<VenueNoEventsDto>>(venuesEntities);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception getting venues", ex);
                return StatusCode(500, "A problem occurred handling your request");
            }
        }

        [HttpGet("{id}", Name = "GetVenue")]
        public IActionResult GetVenue(int id, bool includeEvents = false)
        {
            try
            {
                var venueEntity = _repository.GetVenue(id, includeEvents);
                
                if (venueEntity == null)
                {
                    return NotFound();
                }

                if (includeEvents)
                {
                    var venueResult = Mapper.Map<VenueDto>(venueEntity);
                    return Ok(venueResult);
                }

                var venueNoEventsResult = Mapper.Map<VenueNoEventsDto>(venueEntity);

                return Ok(venueNoEventsResult);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception getting venue with id {id}", ex);
                return StatusCode(500, "A problem occurred handling your request");
            }
        }

        [HttpPost]
        public IActionResult CreateVenue([FromBody] VenueCreateDto venue)
        {
            try
            {
                if (venue == null)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var venueToAdd = Mapper.Map<Entities.Venue>(venue);

                _repository.AddVenue(venueToAdd);

                if (!_repository.Save())
                {
                    return StatusCode(500, "A problem occurred adding the venue");
                }

                var newVenue = Mapper.Map<VenueDto>(venueToAdd);

                return CreatedAtRoute("GetVenue", new { newVenue.Id }, newVenue);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception adding venue", ex);
                return StatusCode(500, "A problem occurred handling your request");
            }
        }

        [HttpPatch("{venueId}")]
        public IActionResult UpdateVenue(int venueId,[FromBody] JsonPatchDocument<VenueUpdateDto> patchDocument)
        {
            try
            {
                if (patchDocument == null)
                {
                    return BadRequest();
                }

                var venueEntity = _repository.GetVenue(venueId);
                if (venueEntity == null)
                {
                    return NotFound();
                }

                var venueToPatch = Mapper.Map<VenueUpdateDto>(venueEntity);

                patchDocument.ApplyTo(venueToPatch, ModelState);

                TryValidateModel(venueToPatch);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Mapper.Map(venueToPatch, venueEntity);

                if (!_repository.Save())
                {
                    return StatusCode(500, "A problem occurred handling your request");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception patching venue id {venueId}", ex);
                return StatusCode(500, "A problem occurred handling your request");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVenue(int id)
        {
            try
            {
                var venueEntity = _repository.GetVenue(id);
                if (venueEntity == null)
                {
                    return NotFound();
                }

                _repository.DeleteVenue(venueEntity);

                if (!_repository.Save())
                {
                    return StatusCode(500, "A problem occurred handling your request");
                }

                _mailService.Send("Venue Deleted", $"Venue {venueEntity.VenueName} with id {id} was deleted.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception deleting venue", ex);
                return StatusCode(500, "A problem occurred handling your request");
            }
        }
    }
}
