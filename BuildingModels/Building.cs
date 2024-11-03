using Core.Entity;

namespace BuildingModels;

public partial class Building : MasterDataEntityBase
{
    public string? Name { get; set; }
    public int? NumberFloor { get; set; }
    public int? NumberApartment { get; set; }
    public Guid? ProjectId { get; set; }

}
