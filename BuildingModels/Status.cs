using Core.Entity;

namespace BuildingModels;

public partial class Status : MasterDataEntityBase
{
    public string? StatusName { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
