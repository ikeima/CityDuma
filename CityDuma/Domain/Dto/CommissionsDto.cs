using System.Collections.Generic;

namespace CityDuma.Domain.Dto
{
    public class CommissionsDto
    {
        public short? CodeMembersCommission { get; set; }
        public string Direction { get; set; }
        public short ChairmanCode { get; set; }
        public ChairmanDto Chairman { get; set; }
    }
}
