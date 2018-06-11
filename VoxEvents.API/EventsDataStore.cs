using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxEvents.API.Models;

namespace VoxEvents.API
{
    public class EventsDataStore
    {
        public static EventsDataStore Current { get; } = new EventsDataStore();

        public List<EventDto> Events { get; set; }
        public List<MemberDto> Members { get; set; }
        public List<VenueDto> Venues { get; set; }

        public EventsDataStore()
        {
            Venues = new List<VenueDto>()
            {
                new VenueDto()
                {
                    Id = 1,
                    VenueName = "St Martins Mullumbimby"
                },
                new VenueDto()
                {
                    Id = 2,
                    VenueName = "Byron Theatre"
                },
                new VenueDto()
                {
                    Id = 3,
                    VenueName = "Mullumbimby Civic Centre"
                }
            };

            Events = new List<EventDto>()
            {
                new EventDto()
                {
                    Id = 1,
                    EventName = "Into the Light",
                    EventVenue = "St Marys Ballina",
                    EventDate = new DateTime(2018, 6, 23),
                    RehearsalTime = new DateTime(2018, 6, 23, 13, 0, 0),
                    PerformanceTime = new DateTime(2018, 6, 23, 15, 0, 0)
                },
                new EventDto()
                {
                    Id  = 2,
                    EventName = "Into the Light",
                    EventVenue = "St Carthages Lismore",
                    EventDate = new DateTime(2018, 6, 24),
                    RehearsalTime = new DateTime(2018, 6, 24, 14, 0, 0),
                    PerformanceTime = new DateTime(2018, 6, 24, 16, 0, 0)
                },
                new EventDto()
                {
                    Id = 3,
                    EventName = "The Events",
                    EventVenue = "Lismore City Hall",
                    EventDate = new DateTime(2018,7,20),
                    RehearsalTime = new DateTime(2018,7,20,13,0,0),
                    PerformanceTime = new DateTime(2018,7,20,18,0,0)
                }
            };

            Members = new List<MemberDto>()
            {
                new MemberDto()
                {
                    Id = 1,
                    FirstName = "Geraldine",
                    LastName = "Doogue",
                    Email = "geraldine@voxcaldera.ort",
                    Phone = "",
                    Part = Parts.Bass,
                    Availabilities = new List<AvailabilityForMemberDto>()
                    {
                        new AvailabilityForMemberDto()
                        {
                            EventId = 1,
                            Available = false
                        },
                        new AvailabilityForMemberDto()
                        {
                            EventId = 2,
                            Available = true
                        },
                        new AvailabilityForMemberDto()
                        {
                            EventId = 3,
                            Available = true
                        }
                    }
                },
                new MemberDto()
                {
                    Id = 2,
                    FirstName = "Julie",
                    LastName = "Andrews",
                    Email = "julie@voxcaldera.org",
                    Phone = "0455123456",
                    Part = Parts.Soprano,
                    Availabilities = new List<AvailabilityForMemberDto>()
                    {
                        new AvailabilityForMemberDto()
                        {
                            EventId = 1,
                            Available = true
                        },
                        new AvailabilityForMemberDto()
                        {
                            EventId = 3,
                            Available = true
                        }
                    }
                },
                new MemberDto()
                {
                    Id = 3,
                    FirstName = "Natalie",
                    LastName = "Wood",
                    Email = "natalie@voxcaldera.org",
                    Phone = "",
                    Part = Parts.Alto,
                    Availabilities = new List<AvailabilityForMemberDto>()
                    {
                        new AvailabilityForMemberDto()
                        {
                            EventId = 1,
                            Available = true
                        },
                        new AvailabilityForMemberDto()
                        {
                            EventId = 2,
                            Available = false
                        }
                    }
                },
                new MemberDto()
                {
                    Id = 4,
                    FirstName = "George",
                    LastName = "Clooney",
                    Email = "george@voxcaldera.org",
                    Phone = "0455987665",
                    Part = Parts.Bass,
                    Availabilities = new List<AvailabilityForMemberDto>()
                    {
                        new AvailabilityForMemberDto()
                        {
                            EventId = 1,
                            Available = true
                        },
                        new AvailabilityForMemberDto()
                        {
                            EventId = 2,
                            Available = true
                        },
                        new AvailabilityForMemberDto()
                        {
                            EventId = 3,
                            Available = true
                        }
                    }
                },
                new MemberDto()
                {
                    Id = 5,
                    FirstName = "Brad",
                    LastName = "Pitt",
                    Email = "brad@voxcaldera.org",
                    Phone = "0455582452",
                    Part = Parts.Tenor,
                    Availabilities = new List<AvailabilityForMemberDto>()
                    {
                        new AvailabilityForMemberDto()
                        {
                            EventId = 1,
                            Available = true
                        }
                    }
                },
                new MemberDto()
                {
                    Id = 6,
                    FirstName = "Kim",
                    LastName = "Novak",
                    Email = "kim@voxcaldera.org",
                    Phone = "0455195548",
                    Part = Parts.Alto,
                    Availabilities = new List<AvailabilityForMemberDto>()
                    {

                    }
                }
            };
        }
    }
}
