using Core.Entity;

namespace BuildingModels;

public partial class ServiceContract : MasterDataEntityBase
{
    public bool RenewStatus { get; set; }
    public bool Canceled { get; set; }
    public DateTime? LastRenewalDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? Status { get; set; }
    public int? Quantity { get; set; }
    public bool IsApprove { get; set; }
    public Guid? ApartmentId { get; set; }
    public Guid? ServiceId { get; set; }

    public virtual Apartment? Apartment { get; set; } = null!;
    public virtual Service? Service { get; set; } = null!;
}
