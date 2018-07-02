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
        Venue GetVenue(int venueId, bool includeEvents);
        IEnumerable<VoxEvent> GetVoxEvents();
        VoxEvent GetVoxEvent(int voxEventId, bool includeAvailabilities);
        IEnumerable<Availability> GetVoxEventAllAvailabilities(int voxEventId);
        IEnumerable<Member> GetMembers();
        IEnumerable<Availability> GetMemberAllAvailabilities(int memberId);
        Member GetMember(int memberId, bool includeAvailabilities);
        Availability GetMemberAvailability(int memberId, int voxEventId);
        bool MemberExists(int memberId);
        bool VoxEventExists(int voxEventId);
        bool AvailabilityExists(int memberId, int voxEventId);
        void AddMemberAvailability(int memberId, Availability availability);
        bool Save();
    }
}
