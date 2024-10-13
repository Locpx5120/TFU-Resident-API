using Core.Entity;

namespace BuildingModels;

public partial class Service : MasterDataEntityBase
{
    public string? ServiceName { get; set; }

    public string? ServiceType { get; set; }
}
