using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxEvents.API.Models
{
    public enum Parts
    {
        Soprano,
        Alto,
        Tenor,
        Bass
    }

    public class MemberDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Parts Part { get; set; }

        public int NumberOfAvailabilities { get
            {
                return Availabilities.Count();
            }
        }

        public ICollection<AvailabilityForMemberDto> Availabilities { get; set; }
            = new List<AvailabilityForMemberDto>();
    }
}
