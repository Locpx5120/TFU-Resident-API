using Core.Entity;

namespace BuildingModels;

public partial class Task : MasterDataEntityBase
{
    public string? TaskName { get; set; }
    public string? Description { get; set; }
    public string? TaskType { get; set; }

    public virtual ICollection<Assigment> Assigments { get; set; } = new List<Assigment>();
}
