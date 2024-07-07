using CityInfor.API.Entities;
using CityInfor.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;

namespace CityInfor.DbContexts
{
    public class CityInfoContext : DbContext
    {
        public CityInfoContext(DbContextOptions<CityInfoContext> options)
            :base(options)
        {   
        }

        public DbSet<City> Cities { get; set; } 
        public DbSet<PointOfInterest> PointOfInterests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City { Id = 1, Name = "Vilnius", Description = "The Capital city"},
                new City { Id = 2, Name = "Kaunas", Description = "The second biggest city in Lithuania" },
                new City { Id = 3, Name = "Klapedia", Description = "at the shore of the baltic sea, it's the ideal place to enjoy beach during summer"}
             );

            modelBuilder.Entity<PointOfInterest>().HasData(
                new PointOfInterest("Gedimino Bokstas") { Id = 1, CityId = 1, Description = "Iconic point in vilnius" },
                new PointOfInterest("Three Statues") { Id = 2, CityId = 1, Description = "Iconic point in vilnius" },
                new PointOfInterest("Kaunas Airport") { Id = 3, CityId = 2, Description = "Kaunas international airport" },
                new PointOfInterest("Kaunas Central Station") { Id = 4, CityId = 2, Description = "Kaunas central rails stations" },
                new PointOfInterest("Klaipeda Beach") { Id = 5, CityId = 3, Description = "Klaipeda international airport" },
                new PointOfInterest("Palanga Beach") { Id = 6, CityId = 3, Description = "Palanga central rails stations" }
            );
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
                optionsBuilder.UseSqlServer("Server=(local);Database=CityInfo;TrustServerCertificate=True;Trusted_Connection=True;");
        }
    }
}
