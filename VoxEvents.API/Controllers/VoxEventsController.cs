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
    [Route("api/voxevents")]
    public class VoxEventsController : Controller
    {
        private readonly IVoxEventsRepository _repository;
        private readonly ILogger<VoxEventsController> _logger;

        public VoxEventsController(IVoxEventsRepository repository, ILogger<VoxEventsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetVoxEvents()
        {
            try
            {
                var voxEventEntities = _repository.GetVoxEvents();

                var results = Mapper.Map<IEnumerable<VoxEventNoAvailabilitiesDto>>(voxEventEntities);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception getting events", ex);
                return StatusCode(500, "A problem occurred handling your request");
            }
        }

        [HttpGet("{id}", Name = "GetVoxEvent")]
        public IActionResult GetVoxEvent(int id, bool includeAvailabilities = false)
        {
            try
            {
                var voxEventEntity = _repository.GetVoxEvent(id, includeAvailabilities);

                if (voxEventEntity == null)
                {
                    return NotFound();
                }

                if (includeAvailabilities)
                {
                    var voxEventResult = Mapper.Map<VoxEventDto>(voxEventEntity);

                    return Ok(voxEventResult);
                }

                var voxEventNoAvailabilitiesResult = Mapper.Map<VoxEventNoAvailabilitiesDto>(voxEventEntity);
                return Ok(voxEventNoAvailabilitiesResult);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception getting event id {id}", ex);
                return StatusCode(500, "A problem occurred handling your request");
            }
        }
        [HttpPost]
        public IActionResult CreateVoxEvent([FromBody] VoxEventCreateDto voxEvent)
        {
            try
            {
                if (voxEvent == null)
                {
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var voxEventToAdd = Mapper.Map<Entities.VoxEvent>(voxEvent);

                _repository.AddVoxEvent(voxEventToAdd);

                if (!_repository.Save())
                {
                    return StatusCode(500, "A problem occurred handling your request");
                }

                var newVoxEvent = Mapper.Map<VoxEventDto>(voxEventToAdd);

                return CreatedAtRoute("GetVoxEvent", new { id = newVoxEvent.Id }, newVoxEvent);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception creating Vox event", ex);
                return StatusCode(500, "A problem occurred handling your request");
            }
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateVoxEvent(int id, [FromBody] JsonPatchDocument<VoxEventUpdateDto> patchDocument)
        {
            try
            {
                if (patchDocument == null)
                {
                    return BadRequest();
                }

                var voxEventEntity = _repository.GetVoxEvent(id);
                if (voxEventEntity == null)
                {
                    return NotFound();
                }

                var voxEventToPatch = Mapper.Map<VoxEventUpdateDto>(voxEventEntity);

                patchDocument.ApplyTo(voxEventToPatch, ModelState);

                TryValidateModel(voxEventToPatch);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Mapper.Map(voxEventToPatch, voxEventEntity);

                if (!_repository.Save())
                {
                    return StatusCode(500, "A problem occurred handling your request");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception patching Vox event", ex);
                return StatusCode(500, "A problem occurred handling your request");
            }
        }
    }
}
