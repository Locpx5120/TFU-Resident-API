using Core.Entity;

namespace BuildingModels;

public partial class Notify : MasterDataEntityBase
{
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }

    public Guid NotifyCategoryId { get; set; }
    public Guid? AssigmentId { get; set; }

    public virtual Assigment? Assigment { get; set; }
    public virtual NotifyCategory NotifyCategory { get; set; } = null!;
}
