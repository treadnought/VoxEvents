using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxEvents.API.Entities;

namespace VoxEvents.API.Services
{
    public class VoxEventsRepository : IVoxEventsRepository
    {
        private readonly VoxEventsContext _context;

        public VoxEventsRepository(VoxEventsContext context)
        {
            _context = context;
        }

        public bool MemberExists(int memberId)
        {
            return _context.Members.Any(m => m.Id == memberId);
        }

        public Member GetMember(int memberId, bool includeAvailabilities)
        {
            if (includeAvailabilities)
            {
                return _context.Members.Include(m => m.Availabilities)
                    .Where(m => m.Id == memberId).FirstOrDefault();
            }

            return _context.Members.Where(m => m.Id == memberId).FirstOrDefault();
        }

        public Availability GetMemberAvailability(int memberId, int voxEventId)
        {
            return _context.Availabilities
                .Where(a => a.MemberId == memberId && a.VoxEventId == voxEventId).FirstOrDefault();
        }

        public IEnumerable<Member> GetMembers()
        {
            return _context.Members.OrderBy(m => m.LastName).ToList();
        }

        public IEnumerable<Venue> GetVenues()
        {
            return _context.Venues.OrderBy(v => v.VenueName).ToList();
        }

        public VoxEvent GetVoxEvent(int voxEventId, bool includeAvailabilities)
        {
            if (includeAvailabilities)
            {
                return _context.VoxEvents.Include(e => e.Availabilities)
                    .Where(e => e.Id == voxEventId).FirstOrDefault();
            }

            return _context.VoxEvents.Where(e => e.Id == voxEventId).FirstOrDefault();
        }

        public IEnumerable<VoxEvent> GetVoxEvents()
        {
            return _context.VoxEvents.OrderBy(v => v.EventDate).ToList();
        }

        public IEnumerable<Availability> GetMemberAllAvailabilities(int memberId)
        {
            return _context.Availabilities.Where(a => a.MemberId == memberId).ToList();
        }

        public bool VoxEventExists(int voxEventId)
        {
            return _context.VoxEvents.Any(v => v.Id == voxEventId);
        }

        public IEnumerable<Availability> GetVoxEventAllAvailabilities(int voxEventId)
        {
            return _context.Availabilities.Where(a => a.VoxEventId == voxEventId);
        }

        public Venue GetVenue(int venueId, bool includeEvents)
        {
            if (includeEvents)
            {
                return _context.Venues.Include(v => v.VoxEvents)
                    .Where(v => v.Id == venueId).FirstOrDefault();
            }
            return _context.Venues.Where(v => v.Id == venueId).FirstOrDefault();
        }

        public void AddMemberAvailability(int memberId, Availability availability)
        {
            var member = GetMember(memberId, false);

            member.Availabilities.Add(availability);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public bool AvailabilityExists(int memberId, int voxEventId)
        {
            var availabilityExists = _context.Availabilities.Any(a => a.MemberId == memberId && a.VoxEventId == voxEventId);
            return availabilityExists;
        }
    }
}
