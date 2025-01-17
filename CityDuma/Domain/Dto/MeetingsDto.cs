using CityDuma.Entities;
using System;

namespace CityDuma.Domain.Dto
{
    public class MeetingsDto
    {
        public short MeetingCode { get; set; }
        public CommissionsDto Commission { get; set; }   
        public OrganizersDto Organizer { get; set; }
        public DateTime? DateOfMeeting { get; set; }
        public TimeSpan? StartTime { get; set; }
        public int? Duration { get; set; }
    }
}
