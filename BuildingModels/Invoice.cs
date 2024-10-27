using Core.Entity;

namespace BuildingModels;

public partial class Invoice : MasterDataEntityBase
{
    public decimal TotalAmount { get; set; }
    public bool PaidStatus { get; set; }
    public DateTime? PaidDate { get; set; }
    public DateTime? IssueDate { get; set; }
    public DateTime? DueDate { get; set; }

    public Guid ServiceContractId { get; set; }

    public Guid UserId { get; set; }

    public virtual User User { get; set; }

    public virtual ServiceContract ServiceContract { get; set; }
}
