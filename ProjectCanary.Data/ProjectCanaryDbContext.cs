using Microsoft.EntityFrameworkCore;
using ProjectCanary.Data.Entities;

namespace ProjectCanary.Data
{
    public class ProjectCanaryDbContext : DbContext
    {

        public DbSet<EmissionSite> EmissionSites { get; set; }

        public DbSet<EquipmentGroup> EquipmentGroups { get; set; }
        
        public DbSet<MeasuredEmission> MeasuredEmissions { get; set; }

        public DbSet<EstimatedEmission> EstimatedEmissions { get; set; }

        public ProjectCanaryDbContext(DbContextOptions<ProjectCanaryDbContext> options) : base(options)
        {
            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmissionSite>().ToTable("emission_sites");
            modelBuilder.Entity<EquipmentGroup>().ToTable("equipment_groups");
            modelBuilder.Entity<MeasuredEmission>().ToTable("measured_emissions");
            modelBuilder.Entity<EstimatedEmission>().ToTable("estimated_emissions");

            modelBuilder.Entity<MeasuredEmission>()
                .HasOne(e => e.EmissionSite)
                .WithMany()
                .HasForeignKey(e => e.SiteId);

            modelBuilder.Entity<MeasuredEmission>()
                .HasOne(e => e.EquipmentGroup)
                .WithMany()
                .HasForeignKey(e => e.EquipmentGroupId);

            modelBuilder.Entity<EstimatedEmission>()
                .HasOne(e => e.EmissionSite)
                .WithMany()
                .HasForeignKey(e => e.SiteId);

            modelBuilder.Entity<EstimatedEmission>()
                .HasOne(e => e.EquipmentGroup)
                .WithMany()
                .HasForeignKey(e => e.EquipmentGroupId);

            modelBuilder.Entity<MeasuredEmission>()
                .HasIndex(me => new { me.EquipmentId, me.MeasurementDate })
                .IsUnique();

            modelBuilder.Entity<EstimatedEmission>()
                .HasIndex(ee => new { ee.SiteId, ee.EquipmentGroupId, ee.EstimateDate })
                .IsUnique();

            modelBuilder.Entity<EmissionSite>().HasData(
                new EmissionSite { SiteId = 1, Name = "Blackstone Pad", Latitude = 39.91413786, Longitude = -80.4778364 },
                new EmissionSite { SiteId = 2, Name = "Cedar Ridge Pad", Latitude = 40.08534879, Longitude = -80.64617783 },
                new EmissionSite { SiteId = 3, Name = "Eagle's Nest Pad", Latitude = 33.14457495, Longitude = -97.44019826 },
                new EmissionSite { SiteId = 4, Name = "Pine Valley Pad", Latitude = 32.75879278, Longitude = -97.26486679 },
                new EmissionSite { SiteId = 5, Name = "Red Rock Pad", Latitude = 31.91391573, Longitude = -93.29114089 },
                new EmissionSite { SiteId = 6, Name = "Ironwood Pad", Latitude = 31.77769914, Longitude = -93.43597152 }
            );

            modelBuilder.Entity<EquipmentGroup>().HasData(
                new EquipmentGroup { EquipmentGroupId = 1, Name = "Sand Traps" },
                new EquipmentGroup { EquipmentGroupId = 2, Name = "Produced Water Tanks" },
                new EquipmentGroup { EquipmentGroupId = 3, Name = "Wells" },
                new EquipmentGroup { EquipmentGroupId = 4, Name = "Slop Tanks" },
                new EquipmentGroup { EquipmentGroupId = 5, Name = "Slug Catchers" },
                new EquipmentGroup { EquipmentGroupId = 6, Name = "GPUs" },
                new EquipmentGroup { EquipmentGroupId = 7, Name = "Dehydrators" },
                new EquipmentGroup { EquipmentGroupId = 8, Name = "Meter Runs" },
                new EquipmentGroup { EquipmentGroupId = 9, Name = "Generators" }
            );

        }
    }
}
