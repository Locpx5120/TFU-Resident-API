using Core.Entity;

namespace BuildingModels;

public partial class Building : MasterDataEntityBase
{
    public string? BuildingName { get; set; }
    public int? NumberFloor { get; set; }
    public int? NumberApartment { get; set; }
    public virtual ICollection<Apartment> Apartments { get; set; } = new List<Apartment>();
    public virtual Guid? PostionId { get; set; }

    public virtual Postion Postion { get; set; } = null!;
}
