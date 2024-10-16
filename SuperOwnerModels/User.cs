using Constant;
using Core.Entity;
using SuperOwnerModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("Users", Schema = Constants.SCHEMA_NAME)]
    public partial class User : MasterDataEntityBase
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; } = "";
        public string Password { get; set; }
        public string? Adress { get; set; } = "";
        public string? NumberCccd { get; set; } = "";
        public Guid RoleId { get; set; }
        public bool IsChangePassword { get; set; } = false;
        public bool Gender { get; set; } = false;
        public DateTime? Dob { get; set; } = DateTime.Now;
        public virtual Role Role { get; set; }
        public virtual IList<Investor> Investors { get; set; }
    }
}
