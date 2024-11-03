using Core.Entity;

namespace BuildingModels;

public partial class Role : MasterDataEntityBase
{
    public string? Name { get; set; }

    public virtual ICollection<Staff> Users { get; set; } = new List<Staff>();
}
