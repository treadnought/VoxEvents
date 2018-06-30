using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoxEvents.API.Entities;

namespace VoxEvents.API.Services
{
    public interface IVoxEventsRepository
    {
        IEnumerable<Venue> GetVenues();
        Venue GetVenue(int venueId);
        IEnumerable<VoxEvent> GetVoxEvents();
        VoxEvent GetVoxEvent(int voxEventId, bool includeAvailabilities);
        IEnumerable<Member> GetMembers();
        Member GetMember(int memberId, bool includeAvailabilities);
        Availability GetMemberAvailability(int memberId, int voxEventId);
    }
}
