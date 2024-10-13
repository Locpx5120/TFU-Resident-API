using Core.Entity;

namespace BuildingModels;

public partial class FinanceBuilding : MasterDataEntityBase
{
    public Guid BuildingId { get; set; }
    public virtual Building Building { get; set; } = null!;
    public Guid FinanceId { get; set; }
    public virtual Finance Finance { get; set; } = null!;
}
