using Core.Entity;

namespace BuildingModels;

public partial class OwnerShip : MasterDataEntityBase
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public Guid? ApartmentId { get; set; }

    public virtual Apartment? Apartment { get; set; }
    public virtual ICollection<Resident> Residents { get; set; } = new List<Resident>();
}
