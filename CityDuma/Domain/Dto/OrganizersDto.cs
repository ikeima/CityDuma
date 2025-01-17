using System;

namespace CityDuma.Domain.Dto
{
    public class OrganizersDto
    {
        public short OrganizerCode { get; set; }
        public string OrganizerFullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PhoneNumber { get; set; }
    }
}
