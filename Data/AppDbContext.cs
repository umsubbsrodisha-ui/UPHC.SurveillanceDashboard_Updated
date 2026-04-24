using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UPHC.SurveillanceDashboard.Models;

namespace UPHC.SurveillanceDashboard.Data
{
   


    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CaseRecord> CaseRecords { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UPHC.SurveillanceDashboard.Models. Facility> Facilities { get; set; }

        public DbSet<FacilityReferral> FacilityReferrals { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 🔗 CaseRecord → facility (Many-to-One)
            builder.Entity<CaseRecord>()
                .HasOne(c => c.Facility)
                .WithMany(u => u.CaseRecords)
                .HasForeignKey(c => c.FacilityId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🔗 Notification → CaseRecord (Many-to-One)
            builder.Entity<Notification>()
                .HasOne(n => n.CaseRecord)
                .WithMany(c => c.Notifications)
                .HasForeignKey(n => n.CaseRecordId)
                .OnDelete(DeleteBehavior.Cascade);

            //ApplicationUser -> Facility (Many-to-one)

            builder.Entity<ApplicationUser>()
    .HasOne(u => u.Facility)
    .WithMany(f=>f.Users)
    .HasForeignKey(u => u.FacilityId)
    .OnDelete(DeleteBehavior.Restrict); 


            //Notification -> Facility (Many-to-one)
            builder.Entity<Notification>()
    .HasOne(n => n.Facility)
    .WithMany(n=>n.Notifications)
    .HasForeignKey(n => n.FacilityId)
    .OnDelete(DeleteBehavior.Restrict);


            // Many referrals to one Facility as far as UPHC is concerned

            builder.Entity<FacilityReferral>()
    .HasOne(fr => fr.UPHC)
    .WithMany(f => f.UPHCReferrals)
    .HasForeignKey(fr => fr.UPHCId)
    .OnDelete(DeleteBehavior.Restrict);

            //Many referrals to one Facility as far as UCHC is concerned
            builder.Entity<FacilityReferral>()
    .HasOne(fr => fr.CHC)
    .WithMany(f => f.CHCReferrals)
    .HasForeignKey(fr => fr.CHCId)
    .OnDelete(DeleteBehavior.Restrict);
           
            
            // A composite unique key is created in Facility Referral table
            builder.Entity<FacilityReferral>()
    .HasIndex(fr => new { fr.UPHCId, fr.CHCId })
    .IsUnique();
            // Seed Facility
            builder.Entity<UPHC.SurveillanceDashboard.Models.Facility>().HasData(



                new UPHC.SurveillanceDashboard.Models.Facility { Id = 1, facilityName = "UCHC Unit-4", Type = facilityType.CHC, facilityAddress = "Unit-4" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 2, facilityName = "UCHC Dumduma", Type = facilityType.CHC, facilityAddress = "Dumduma" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 3, facilityName = "UCHC Unit-8", Type = facilityType.CHC, facilityAddress = "Unit-8" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 4, facilityName = "UCHC BMC Hospital", Type = facilityType.CHC, facilityAddress = "Old Town,Lingaraj nagar" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id =5 , facilityName = "Unit-3 UPHC",Type=facilityType.UPHC ,facilityAddress = "Kharvela Nagar" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id =6, facilityName = "Unit-4 UPHC", Type = facilityType.UPHC, facilityAddress = "Unit-4,Near AG Colony" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 7, facilityName = "Unit-8 UPHC", Type = facilityType.UPHC, facilityAddress = "Unit-8" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 8, facilityName = "Unit-9 UPHC",Type=facilityType.UPHC, facilityAddress = "Unit-9 Industrial Colony" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 9, facilityName = "Saheed Nagar UPHC", Type = facilityType.UPHC, facilityAddress = "Saheed Nagar" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 10, facilityName = "Satya Nagar UPHC",Type = facilityType.UPHC, facilityAddress = "Satya Nagar,Near Kali Mandir" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 11, facilityName = "Baramunda UPHC", Type = facilityType.UPHC, facilityAddress = "Rental Colony, IRC Village" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 12, facilityName = "IRC Village UPHC", Type = facilityType.UPHC, facilityAddress = "IRC Village, Nayapalli" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 13, facilityName = "Rasulgarh UPHC", Type = facilityType.UPHC, facilityAddress = "GGP Colony, Rasulgarh" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 14, facilityName = "Gadakan UPHC", Type = facilityType.UPHC, facilityAddress = "Gadeswar,Near RI Office Kalarahanga" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 15, facilityName = "Pokhariput UPHC", Type = facilityType.UPHC, facilityAddress = "Pokhariput,Ward No-62" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 16, facilityName = "Chandrasekharpur UPHC", Type = facilityType.UPHC, facilityAddress = "CS Pur HB Colony,Ward No-8" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 17, facilityName = "Niladri Vihar UPHC", Type = facilityType.UPHC, facilityAddress = "Niladri Vihar, Sector-I" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 18, facilityName = "Bjb Nagar UPHC", Type = facilityType.UPHC, facilityAddress = "BJB Nagar,Near BJB Nagar Hata" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 19, facilityName = "Bhimatangi UPHC", Type = facilityType.UPHC, facilityAddress = "Bhimatangi Housing Board area" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 20, facilityName = "Kapilaprasad UPHC", Type = facilityType.UPHC, facilityAddress = "Kapilaprasad Village area" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 21, facilityName = "Badagada UPHC", Type = facilityType.UPHC, facilityAddress = "Badagada Village,Brit area" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 22, facilityName = "Jharapada UPHC", Type = facilityType.UPHC, facilityAddress = "Jharapada Village area" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 23, facilityName = "Bharatpur UPHC", Type = facilityType.UPHC, facilityAddress = "Bharatpur Slum,Mahalaxmi Vihar" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 24, facilityName = "Patia UPHC", Type = facilityType.UPHC, facilityAddress = "Patia Village,Damana area" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 25, facilityName = "VSS Nagar UPHC", Type = facilityType.UPHC, facilityAddress = "VSS Nagar Housing Board area" },
                new UPHC.SurveillanceDashboard.Models.Facility { Id = 26, facilityName = "Laxmisagar UPHC", Type = facilityType.UPHC, facilityAddress = "Laxmisagar Village area" }
            );
        }
    }
}
