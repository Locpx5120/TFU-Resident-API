using Core.Entity;

namespace BuildingModels;

public partial class Resident : MasterDataEntityBase
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Fullname { get; set; }
    public int? PhoneNumber { get; set; }
    public DateOnly? Dob { get; set; }
    public DateTime? RegistratorDate { get; set; }

    public Guid? OwnerShipId { get; set; }

    public virtual OwnerShip? OwnerShip { get; set; }

    public virtual ICollection<Living> Livings { get; set; } = new List<Living>();
    public virtual ICollection<RequestComplain> RequestComplains { get; set; } = new List<RequestComplain>();
}
