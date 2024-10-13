using Core.Entity;

namespace BuildingModels;

public partial class Postion : MasterDataEntityBase
{
    public string CodePosition { get; set; } = null!;

    public Guid CustomerId { get; set; }

    public virtual ICollection<Building> Buildings { get; set; } = new List<Building>();
}
