using Core.Entity;

namespace BuildingModels;

public partial class RequestComplain : MasterDataEntityBase
{
    public string? Description { get; set; }
    public int? Status { get; set; }
    public DateTime? DateRequest { get; set; }
    public string? Note { get; set; }

    public Guid? ResidentId { get; set; }

    public virtual Resident? Resident { get; set; }
}
