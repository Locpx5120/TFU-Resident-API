using Core.Entity;

namespace BuildingModels;

public partial class Apartment : MasterDataEntityBase
{
    public Guid ApartmentTypeId { get; set; }
    public virtual ApartmentType ApartmentType { get; set; }

    public double? Price { get; set; }

    public int FloorNumber { get; set; }
    public int RoomNumber { get; set; }

    public Guid BuildingId { get; set; }
    public virtual Building Building { get; set; }

    public virtual ICollection<Living> Livings { get; set; } = new List<Living>();

    public virtual ICollection<OwnerShip> OwnerShips { get; set; } = new List<OwnerShip>();
}
