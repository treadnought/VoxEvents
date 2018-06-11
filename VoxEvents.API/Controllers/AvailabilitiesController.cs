﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxEvents.API.Models;

namespace VoxEvents.API.Controllers
{
    [Route("api")]
    public class AvailabilitiesController : Controller
    {
        [HttpGet("events/{eventId}/availabilities")]
        public IActionResult GetEventAllAvailabilities(int eventId)
        {
            var eventToCheck = EventsDataStore.Current.Events.FirstOrDefault(e => e.Id == eventId);

            if (eventToCheck == null)
            {
                return NotFound();
            }

            return Ok(eventToCheck.Availabilities);
        }

        [HttpGet("members/{memberId}/availabilities")]
        public IActionResult GetMemberAllAvailabilities(int memberId)
        {
            var member = EventsDataStore.Current.Members.FirstOrDefault(m => m.Id == memberId);

            if (member == null)
            {
                return NotFound();
            }

            return Ok(member.Availabilities);
        }

        [HttpGet("members/{memberId}/availabilities/{eventId}", Name = "GetAvailability")]
        public IActionResult GetMemberAvailability(int memberId, int eventId)
        {
            var member = EventsDataStore.Current.Members.FirstOrDefault(m => m.Id == memberId);
            if (member == null)
            {
                return NotFound();
            }

            var availability = member.Availabilities.FirstOrDefault(e => e.EventId == eventId);
            if (availability == null)
            {
                return NotFound();
            }

            return Ok(availability);
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

            var member = EventsDataStore.Current.Members.FirstOrDefault(m => m.Id == memberId);
            if (member == null)
            {
                return NotFound();
            }

            var finalMemberAvailability = new MemberAvailabilityDto()
            {
                EventId = availability.EventId,
                Available = availability.Available
            };

            member.Availabilities.Add(finalMemberAvailability);

            return CreatedAtRoute("GetAvailability", new
                { memberId, eventId = availability.EventId }, 
                finalMemberAvailability);
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
