using AutoMapper;
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
    [Route("api/events")]
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

            //var eventToReturn = VoxEventsDataStore.Current.Events.FirstOrDefault(e => e.Id == id);
            //if (eventToReturn == null)
            //{
            //    return NotFound();
            //}
            //return Ok(eventToReturn);
        }

        //[HttpPost]
        //public IActionResult CreateEvent([FromBody] VoxEventCreateDto choirEvent)
        //{
        //    if (choirEvent == null)
        //    {
        //        return BadRequest();
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }


        //}
    }
}
