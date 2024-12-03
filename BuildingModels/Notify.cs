using Core.Entity;

namespace BuildingModels;

public partial class Notify : MasterDataEntityBase
{
    public DateTime? Date { get; set; }
    public TimeSpan? Time { get; set; }

    public Guid NotifyCategoryId { get; set; }
    public virtual NotifyCategory NotifyCategory { get; set; } = null!;
    public Guid RoleId { get; set; }
    public Guid BuildingId { get; set; }
    public string Title { get; set; }
    public string ShortContent { get; set; }
    public string LongContent { get; set; }
    public string UrlImg { get; set; }
    public int Status { get; set; }

}
