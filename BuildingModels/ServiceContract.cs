using Core.Entity;

namespace BuildingModels;

public partial class ServiceContract : MasterDataEntityBase
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? Status { get; set; }
    public int? Quantity { get; set; }
    public string? Note { get; set; } = string.Empty;
    public string? NoteDetail { get; set; } = string.Empty;
    public string? NoteFeedbackCuDan { get; set; } = string.Empty;
    public string? NoteKyThuat { get; set; } = string.Empty;
    public string? NoteFeedbackHanhChinh { get; set; } = string.Empty;
    public Guid? ApartmentId { get; set; }
    public Guid? ServiceId { get; set; }
    public Guid? VehicleId { get; set; }
    public Guid? PackageServiceId { get; set; }
    public Guid? LivingId { get; set; }
    public virtual Living Living { get; set; }

    public virtual PackageService? PackageService { get; set; }

    public virtual Apartment? Apartment { get; set; } = null!;
    public virtual Service? Service { get; set; } = null!;
    public virtual Vehicle? Vehicle { get; set; }
    public virtual ICollection<Assigment> Assigments { get; set; } = new List<Assigment>();
}
