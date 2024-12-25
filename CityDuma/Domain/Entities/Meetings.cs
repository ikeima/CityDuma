namespace CityDuma.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Meetings
    {
        [Key]
        public short MeetingCode { get; set; }

        public short? Commission { get; set; }

        public short? Organizer { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOfMeeting { get; set; }

        public TimeSpan? StartTime { get; set; }

        public int? Duration { get; set; }

        public virtual Commissions Commissions { get; set; }

        public virtual Organizers Organizers { get; set; }
    }
}
