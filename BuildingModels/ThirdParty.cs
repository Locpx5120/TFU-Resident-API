using Core.Entity;

namespace BuildingModels;

public partial class ThirdParty : MasterDataEntityBase
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Contact { get; set; }
}
