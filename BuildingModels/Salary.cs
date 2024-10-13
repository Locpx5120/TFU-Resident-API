using Core.Entity;

namespace BuildingModels;

public partial class Salary : MasterDataEntityBase
{
    public double? AmoutSalary { get; set; }

    public Guid? RoleId { get; set; }

    public virtual Role Role { get; set; } = null!;
}
