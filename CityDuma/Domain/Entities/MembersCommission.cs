namespace CityDuma.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MembersCommission")]
    public partial class MembersCommission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short CodeMembersDuma { get; set; }

        public short? CodeMembersCommission { get; set; }

        public virtual Commissions Commissions { get; set; }

        public virtual MembersDuma MembersDuma { get; set; }
    }
}
