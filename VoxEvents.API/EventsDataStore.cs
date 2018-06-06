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

        public List<EventDTO> Events { get; set; }
        public List<MemberDTO> Members { get; set; }

        public EventsDataStore()
        {
            Events = new List<EventDTO>()
            {
                new EventDTO()
                {
                    Id = 1,
                    EventName = "Into the Light",
                    EventVenue = "St Marys Ballina",
                    EventDate = new DateTime(2018, 6, 23),
                    RehearsalTime = new DateTime(2018, 6, 23, 13, 0, 0),
                    PerformanceTime = new DateTime(2018, 6, 23, 15, 0, 0)
                },
                new EventDTO()
                {
                    Id  = 2,
                    EventName = "Into the Light",
                    EventVenue = "St Carthages Lismore",
                    EventDate = new DateTime(2018, 6, 24),
                    RehearsalTime = new DateTime(2018, 6, 24, 14, 0, 0),
                    PerformanceTime = new DateTime(2018, 6, 24, 16, 0, 0)
                },
                new EventDTO()
                {
                    Id = 3,
                    EventName = "The Events",
                    EventVenue = "Lismore City Hall",
                    EventDate = new DateTime(2018,7,20),
                    RehearsalTime = new DateTime(2018,7,20,13,0,0),
                    PerformanceTime = new DateTime(2018,7,20,18,0,0)
                }
            };

            Members = new List<MemberDTO>()
            {
                new MemberDTO()
                {
                    Id = 1,
                    FirstName = "Ian",
                    LastName = "Bowles",
                    Email = "ianabowles@yahoo.com.au",
                    Phone = "",
                    Part = Parts.Bass
                },
                new MemberDTO()
                {
                    Id = 2,
                    FirstName = "Sandy",
                    LastName = "Cochrane",
                    Email = "sandy.cochrane@hotmail.com",
                    Phone = "0427495076",
                    Part = Parts.Soprano
                },
                new MemberDTO()
                {
                    Id = 3,
                    FirstName = "Heather",
                    LastName = "Ellemor-Collins",
                    Email = "hellemorcollins@gmail.com",
                    Phone = "",
                    Part = Parts.Alto
                },
                new MemberDTO()
                {
                    Id = 4,
                    FirstName = "David",
                    LastName = "Ellemor-Collins",
                    Email = "davidec.email@gmail.com",
                    Phone = "0457670122",
                    Part = Parts.Bass
                },
                new MemberDTO()
                {
                    Id = 5,
                    FirstName = "Chris",
                    LastName = "Harris",
                    Email = "harr59@hotmail.com",
                    Phone = "0457365015",
                    Part = Parts.Tenor
                },
                new MemberDTO()
                {
                    Id = 6,
                    FirstName = "Laura",
                    LastName = "Hymers",
                    Email = "lauramhy@gmail.com",
                    Phone = "0457365015",
                    Part = Parts.Alto
                },
            };
        }
    }
}
