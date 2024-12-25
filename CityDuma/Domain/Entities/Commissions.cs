namespace CityDuma.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Commissions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Commissions()
        {
            Meetings = new HashSet<Meetings>();
            MembersCommission = new HashSet<MembersCommission>();
        }

        [Key]
        public short CodeMembersCommission { get; set; }

        public short ChairmanCode { get; set; }

        [StringLength(20)]
        public string Direction { get; set; }

        public virtual MembersDuma MembersDuma { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Meetings> Meetings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MembersCommission> MembersCommission { get; set; }
    }
}
