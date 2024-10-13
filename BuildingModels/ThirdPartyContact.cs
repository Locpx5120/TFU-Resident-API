using Core.Entity;

namespace BuildingModels;

public partial class ThirdPartyContact : MasterDataEntityBase
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public Guid? StaffId { get; set; }
    public Guid? BuildingId { get; set; }

    public virtual Staff Staff { get; set; } = null!;
    public virtual Building Building { get; set; } = null!;
}
