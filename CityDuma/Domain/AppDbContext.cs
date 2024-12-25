using System.Data.Entity;

namespace CityDuma.Entities
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
            : base("name=City_DumaEntities")
        {
        }

        public virtual DbSet<Commissions> Commissions { get; set; }
        public virtual DbSet<Meetings> Meetings { get; set; }
        public virtual DbSet<MembersDuma> MembersDuma { get; set; }
        public virtual DbSet<Organizers> Organizers { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<MembersCommission> MembersCommission { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Commissions>()
                .HasMany(e => e.Meetings)
                .WithOptional(e => e.Commissions)
                .HasForeignKey(e => e.Commission)
                .WillCascadeOnDelete();

            modelBuilder.Entity<MembersDuma>()
                .HasMany(e => e.Commissions)
                .WithRequired(e => e.MembersDuma)
                .HasForeignKey(e => e.ChairmanCode);

            modelBuilder.Entity<MembersDuma>()
                .HasOptional(e => e.MembersCommission)
                .WithRequired(e => e.MembersDuma);

            modelBuilder.Entity<Organizers>()
                .HasMany(e => e.Meetings)
                .WithOptional(e => e.Organizers)
                .HasForeignKey(e => e.Organizer)
                .WillCascadeOnDelete();
        }
    }
}
