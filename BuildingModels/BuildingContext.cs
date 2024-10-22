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
        optionsBuilder.UseSqlServer("server=.;database=DBJustBlog;Trusted_Connection=True;TrustServerCertificate=True");
    }

    //add-migration DB

    public virtual DbSet<Apartment> Apartments { get; set; }

    public virtual DbSet<Assigment> Assigments { get; set; }

    public virtual DbSet<Building> Buildings { get; set; }

    public virtual DbSet<Contributor> Contributors { get; set; }

    public virtual DbSet<Finance> Finances { get; set; }

    public virtual DbSet<FinanceBuilding> FinanceBuildings { get; set; }

    public virtual DbSet<HandleRequest> HandleRequests { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Living> Livings { get; set; }

    public virtual DbSet<Notify> Notifies { get; set; }

    public virtual DbSet<NotifyCategory> NotifyCategories { get; set; }

    public virtual DbSet<OwnerShip> OwnerShips { get; set; }

    public virtual DbSet<Postion> Postions { get; set; }

    public virtual DbSet<RequestComplain> RequestComplains { get; set; }

    public virtual DbSet<Resident> Residents { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceContract> ServiceContracts { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<ThirdParty> ThirdParties { get; set; }

    public virtual DbSet<ThirdPartyContact> ThirdPartyContacts { get; set; }

    public virtual DbSet<OTPMail> OTPMails { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
