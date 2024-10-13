using Core.Entity;

namespace BuildingModels;

public partial class Invoice : MasterDataEntityBase
{
    public int? Amount { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? TransactionDate { get; set; }

    public Guid StatusId { get; set; }
    public Guid ApartmentId { get; set; }

    public virtual Apartment Apartment { get; set; } = null!;
    public virtual Status Status { get; set; } = null!;
}
