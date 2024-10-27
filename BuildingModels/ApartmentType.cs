using Core.Entity;

namespace BuildingModels;

public partial class ApartmentType : MasterDataEntityBase
{
    public string Name { get; set; }
    public decimal LandArea { get; set; }
}
