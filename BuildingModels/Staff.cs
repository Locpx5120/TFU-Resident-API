using Core.Entity;

namespace BuildingModels;

public partial class Staff : MasterDataEntityBase
{
    public DateTime? HireDate { get; set; }
    public Guid? UserId { get; set; }
    public virtual User? User { get; set; } = null!;
}
