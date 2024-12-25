namespace CityDuma.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MembersDuma")]
    public partial class MembersDuma
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MembersDuma()
        {
            Commissions = new HashSet<Commissions>();
        }

        [Key]
        public short CodeMembersDuma { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Patronymic { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BirthDate { get; set; }

        [StringLength(13)]
        public string PhoneNumber { get; set; }

        public short? Experience { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateEntryOfDuma { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Commissions> Commissions { get; set; }

        public virtual MembersCommission MembersCommission { get; set; }
    }
}
