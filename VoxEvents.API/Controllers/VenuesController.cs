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
    [Route("api/venues")]
    public class VenuesController : Controller
    {
        private readonly ILogger<VenuesController> _logger;
        private readonly IVoxEventsRepository _repository;

        public VenuesController(ILogger<VenuesController> logger, IVoxEventsRepository repository)
        {
            _logger = logger;
            _repository = repository;
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
    }
}
