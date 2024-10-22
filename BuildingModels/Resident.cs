using Core.Entity;

namespace BuildingModels;

public partial class Resident : MasterDataEntityBase
{
    public DateTime? RegistratorDate { get; set; }

    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public Guid? OwnerShipId { get; set; }

    public virtual OwnerShip? OwnerShip { get; set; }

    public virtual ICollection<Living> Livings { get; set; } = new List<Living>();
    public virtual ICollection<RequestComplain> RequestComplains { get; set; } = new List<RequestComplain>();
}
