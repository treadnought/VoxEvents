using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxEvents.API.Entities;

namespace VoxEvents.API
{
    public static class VoxEventsContextExtensions
    {
        public static void EnsureSeedDataForContext(this VoxEventsContext context)
        {
            if (context.Members.Any())
            {
                return;
            }

            var venues = new List<Venue>()
            {
                new Venue()
                {
                    VenueName = "St Martins Mullumbimby",
                    HasPiano = false
                },
                new Venue()
                {
                    VenueName = "Byron Theatre",
                    HasPiano = true
                },
                new Venue()
                {
                    VenueName = "Mullumbimby Civic Centre",
                    HasPiano = false
                }
            };

            var voxEvents = new List<VoxEvent>()
            {
                new VoxEvent()
                {
                    EventName = "Into the Light",
                    VenueId = 1,
                    EventDate = new DateTime(2018, 6, 23),
                    RehearsalTime = new DateTime(2018, 6, 23, 13, 0, 0),
                    PerformanceTime = new DateTime(2018, 6, 23, 15, 0, 0)
                },
                new VoxEvent()
                {
                    EventName = "Into the Light",
                    VenueId = 2,
                    EventDate = new DateTime(2018, 6, 24),
                    RehearsalTime = new DateTime(2018, 6, 24, 14, 0, 0),
                    PerformanceTime = new DateTime(2018, 6, 24, 16, 0, 0)
                },
                new VoxEvent()
                {
                    EventName = "The Events",
                    VenueId = 3,
                    EventDate = new DateTime(2018,7,20),
                    RehearsalTime = new DateTime(2018,7,20,13,0,0),
                    PerformanceTime = new DateTime(2018,7,20,18,0,0)
                }
            };

            var members = new List<Member>()
            {
                new Member()
                {
                    FirstName = "Geraldine",
                    LastName = "Doogue",
                    Email = "geraldine@voxcaldera.ort",
                    Phone = "",
                    Part = "Bass",
                    Availabilities = new List<Availability>()
                    {
                        new Availability()
                        {
                            VoxEventId = 1,
                            Available = false
                        },
                        new Availability()
                        {
                            VoxEventId = 2,
                            Available = true
                        },
                        new Availability()
                        {
                            VoxEventId = 3,
                            Available = true
                        }
                    }
                },
                new Member()
                {
                    FirstName = "Julie",
                    LastName = "Andrews",
                    Email = "julie@voxcaldera.org",
                    Phone = "0455123456",
                    Part = "Soprano",
                    Availabilities = new List<Availability>()
                    {
                        new Availability()
                        {
                            VoxEventId = 1,
                            Available = true
                        },
                        new Availability()
                        {
                            VoxEventId = 3,
                            Available = true
                        }
                    }
                },
                new Member()
                {
                    FirstName = "Natalie",
                    LastName = "Wood",
                    Email = "natalie@voxcaldera.org",
                    Phone = "",
                    Part = "Alto",
                    Availabilities = new List<Availability>()
                    {
                        new Availability()
                        {
                            VoxEventId = 1,
                            Available = true
                        },
                        new Availability()
                        {
                            VoxEventId = 2,
                            Available = false
                        }
                    }
                },
                new Member()
                {
                    FirstName = "George",
                    LastName = "Clooney",
                    Email = "george@voxcaldera.org",
                    Phone = "0455987665",
                    Part = "Bass",
                    Availabilities = new List<Availability>()
                    {
                        new Availability()
                        {
                            VoxEventId = 1,
                            Available = true
                        },
                        new Availability()
                        {
                            VoxEventId = 2,
                            Available = true
                        },
                        new Availability()
                        {
                            VoxEventId = 3,
                            Available = true
                        }
                    }
                },
                new Member()
                {
                    FirstName = "Brad",
                    LastName = "Pitt",
                    Email = "brad@voxcaldera.org",
                    Phone = "0455582452",
                    Part = "Tenor",
                    Availabilities = new List<Availability>()
                    {
                        new Availability()
                        {
                            VoxEventId = 1,
                            Available = true
                        }
                    }
                },
                new Member()
                {
                    FirstName = "Kim",
                    LastName = "Novak",
                    Email = "kim@voxcaldera.org",
                    Phone = "0455195548",
                    Part = "Alto",
                    Availabilities = new List<Availability>()
                    {

                    }
                }
            };

            context.Venues.AddRange(venues);
            context.VoxEvents.AddRange(voxEvents);
            context.Members.AddRange(members);
            context.SaveChanges();
        }
    }
}
