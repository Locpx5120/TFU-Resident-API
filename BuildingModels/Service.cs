using Core.Entity;

namespace BuildingModels;

public partial class Service : MasterDataEntityBase
{
    public string ServiceName { get; set; }
    public string Description { get; set; }

    public decimal UnitPrice { get; set; }
    public string Unit { get; set; }
    public bool IsPackageAllowed { get; set; }
}
