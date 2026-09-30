using Microsoft.EntityFrameworkCore;
using RentCar.Models;

namespace RentCar.Data
{
    public class RentCarContext : DbContext
    {
        public RentCarContext(DbContextOptions<RentCarContext> options)
            : base(options)
        {
        }
        public DbSet<TrRental> TrRentals { get; set; }
        public DbSet<MsCar> MsCars { get; set; }
        public DbSet<MsCustomer> MsCustomers { get; set; }
        public DbSet<MsCarImages> MsCarImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
 
            modelBuilder.Entity<MsCar>().ToTable("MsCar");

            modelBuilder.Entity<MsCustomer>().ToTable("MsCustomer");

            modelBuilder.Entity<MsCarImages>().ToTable("MsCarImages");
        }
    }

}