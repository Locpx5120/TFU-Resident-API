using Core.Entity;

namespace BuildingModels;

public partial class Resident : MasterDataEntityBase
{
    public DateTime? RegistratorDate { get; set; }

    public Guid? UserId { get; set; }
    public virtual User? User { get; set; } = null!;
    public Guid? OwnerShipId { get; set; }

    public virtual OwnerShip? OwnerShip { get; set; }

    public virtual ICollection<Living> Livings { get; set; } = new List<Living>();
    public virtual ICollection<RequestComplain> RequestComplains { get; set; } = new List<RequestComplain>();
}
