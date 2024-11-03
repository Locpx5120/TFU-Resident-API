using Core.Entity;

namespace BuildingModels;

public partial class Vehicle : MasterDataEntityBase
{
    public Guid? ResidentId { get; set; } // ID của cư dân sở hữu phương tiện
    public string VehicleType { get; set; } = null!; // Loại phương tiện, ví dụ: Honda Airblade
    public string LicensePlate { get; set; } = null!; // Biển số xe

    // Navigation properties
    public virtual Resident? Resident { get; set; } = null!; // Liên kết đến cư dân sở hữu
}
