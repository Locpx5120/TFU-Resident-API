using Core.Entity;

namespace BuildingModels;

public class Staff : MasterDataEntityBase
{
    public DateTime? HireDate { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? IsChangePassword { get; set; }
    public DateTime? Birthday { get; set; }
    public Guid RoleId { get; set; }
    public virtual Role Role { get; set; }
}
