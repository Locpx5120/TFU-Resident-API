using Core.Entity;

namespace BuildingModels;

public partial class Role : MasterDataEntityBase
{
    public string? Name { get; set; }
    public string? Name_En { get; set; }

    public virtual ICollection<Staff> Users { get; set; } = new List<Staff>();
    public virtual ICollection<Notify> Notify { get; set; } = new List<Notify>();
}
