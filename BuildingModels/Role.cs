using Core.Entity;

namespace BuildingModels;

public partial class Role : MasterDataEntityBase
{
    public string? Name { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
