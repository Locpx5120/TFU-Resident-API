using Core.Entity;

namespace BuildingModels;

public partial class OwnerShip : MasterDataEntityBase
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public Guid? ApartmentId { get; set; }

    public virtual Apartment? Apartment { get; set; }
    public Guid? ResidentId { get; set; }
    public virtual Resident Resident { get; set; } = null!;
}
