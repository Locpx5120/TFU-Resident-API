using Microsoft.EntityFrameworkCore;

namespace BuildingModels;

public partial class BuildingContext : DbContext
{

    public BuildingContext()
    {
    }

    public BuildingContext(DbContextOptions options) : base(options)
    {
    }

    public BuildingContext(DbContextOptions<BuildingContext> options, string connectionString)
    : base(options)
    {
        Database.SetConnectionString(connectionString);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source= 202.92.7.204,1437;Initial Catalog=QLToaNhaDb2_1;Persist Security Info=True;User ID=QLToaNhaDb2_1;Password=qe853B5f%;TrustServerCertificate=True");
    }

    // add-migration updateDB15 -Context BuildingContext
    // update-database -Context BuildingContext

    public virtual DbSet<Apartment> Apartments { get; set; }
    public virtual DbSet<Transaction> Transactions { get; set; }
    public virtual DbSet<ApartmentType> ApartmentTypes { get; set; }

    public virtual DbSet<Assigment> Assigments { get; set; }

    public virtual DbSet<Building> Buildings { get; set; }


    public virtual DbSet<Finance> Finances { get; set; }

    public virtual DbSet<FinanceBuilding> FinanceBuildings { get; set; }

    public virtual DbSet<HandleRequest> HandleRequests { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Living> Livings { get; set; }

    public virtual DbSet<Notify> Notifies { get; set; }

    public virtual DbSet<ImgBase> ImgBases { get; set; }

    public virtual DbSet<OwnerShip> OwnerShips { get; set; }


    public virtual DbSet<RequestComplain> RequestComplains { get; set; }

    public virtual DbSet<Resident> Residents { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceContract> ServiceContracts { get; set; }

    public virtual DbSet<PackageService> PackageServices { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<ThirdParty> ThirdParties { get; set; }

    public virtual DbSet<ThirdPartyContact> ThirdPartyContacts { get; set; }
    public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }
    public virtual DbSet<Vehicle> Vehicles { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Specify precision and scale for 'LandArea' on 'ApartmentType'
        //modelBuilder.Entity<ApartmentType>()
        //    .Property(a => a.LandArea)
        //    .HasPrecision(18, 2); // Adjust 18 and 2 to the precision and scale you need

        //// Specify precision and scale for 'TotalAmount' on 'Invoice'
        //modelBuilder.Entity<Invoice>()
        //    .Property(i => i.TotalAmount)
        //    .HasPrecision(18, 2); // Adjust as necessary

        //// Specify precision and scale for 'Discount' on 'PackageService'
        //modelBuilder.Entity<PackageService>()
        //    .Property(p => p.Discount)
        //    .HasPrecision(18, 2); // Adjust as necessary

        //// Specify precision and scale for 'UnitPrice' on 'Service'
        //modelBuilder.Entity<Service>()
        //    .Property(s => s.UnitPrice)
        //    .HasPrecision(18, 2); // Adjust as necessary

        //// Specify precision and scale for 'Price' on 'ThirdPartyContact'
        //modelBuilder.Entity<ThirdPartyContact>()
        //    .Property(t => t.Price)
        //    .HasPrecision(18, 2); // Adjust as necessary
        base.OnModelCreating(modelBuilder);

    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
