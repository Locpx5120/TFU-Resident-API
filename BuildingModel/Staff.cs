using Core.Entity;

namespace BuildingModels;

public partial class Staff : MasterDataEntityBase
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public int? PhoneNumber { get; set; }
    public DateTime? HireDate { get; set; }

    public Guid? RoleId { get; set; }
    public virtual Role? Role { get; set; } = null!;
}
