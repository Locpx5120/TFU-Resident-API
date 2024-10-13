using Core.Entity;

namespace BuildingModels;

public partial class ServiceContract : MasterDataEntityBase
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? Status { get; set; }
    public int? Amount { get; set; }

    public Guid? ApartmentId { get; set; }
    public Guid? ServiceId { get; set; }

    public virtual Apartment? Apartment { get; set; } = null!;
    public virtual Service? Service { get; set; } = null!;
}
