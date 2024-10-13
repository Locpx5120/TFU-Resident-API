using Core.Entity;

namespace BuildingModels;

public partial class NotifyCategory : MasterDataEntityBase
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
