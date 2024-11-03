using Core.Entity;

namespace BuildingModels
{
    public class PackageService : MasterDataEntityBase
    {
        public string Name { get; set; }
        public decimal? Discount { get; set; }
        public int DurationInMonth { get; set; }
    }
}
