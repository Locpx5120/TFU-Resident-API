using Core.Entity;

namespace BuildingModels;

public partial class ThirdPartyContact : MasterDataEntityBase
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal Price { get; set; }
    public string NameService { get; set; }

    public Guid? ThirdPartyId { get; set; }

    public virtual ThirdParty ThirdParty { get; set; } = null!;
}
