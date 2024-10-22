using Core.Entity;

namespace BuildingModels
{
    public partial class User : MasterDataEntityBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? Dob { get; set; }
        public bool IsChangePassword { get; set; } = false;
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; } = null!;

        public virtual ICollection<Staff> Staffs { get; set; } = new List<Staff>();
        public virtual ICollection<Living> Livings { get; set; } = new List<Living>();
        public virtual ICollection<OwnerShip> OwnerShips { get; set; } = new List<OwnerShip>();
    }
}
