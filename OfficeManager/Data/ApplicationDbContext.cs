namespace OfficeManager.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using OfficeManager.Models;

    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Office> Offices { get; set; }

        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<ElectricityMeasurement> ElectricityMeasurements { get; set; }

        public DbSet<TemperatureMeasurement> TemperatureMeasurements { get; set; }

        public DbSet<ElectricityMeter> ElectricityMeters { get; set; }

        public DbSet<TemperatureMeter> TemperatureMeters { get; set; }

        public DbSet<AccountingReport> AccountingReports { get; set; }

        public DbSet<PricesInformation> PricesInformation { get; set; }

        public DbSet<Landlord> Landlords { get; set; }

        public DbSet<InvoiceDescription> InvoiceDescriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PricesInformation>()
                   .HasMany(a => a.AccountingReports)
                   .WithOne(b => b.PricesInformation)
                   .HasForeignKey(b => b.PricesInformationId);

            builder.Entity<Landlord>()
                   .HasMany(a => a.AccountingReports)
                   .WithOne(b => b.Landlord)
                   .HasForeignKey(b => b.LandlordId);

            builder.Entity<Landlord>()
                   .HasOne(a => a.Invoice)
                   .WithOne(b => b.Landlord)
                   .HasForeignKey<Invoice>(b => b.LandlordId);

            builder.Entity<Office>()
                   .HasOne(a => a.ElectricityMeter)
                   .WithOne(b => b.Office)
                   .HasForeignKey<ElectricityMeter>(b => b.OfficeId);
        }
    }
}
