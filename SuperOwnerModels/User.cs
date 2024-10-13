using Constant;
using Core.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using TFU_Resident_API.Entity;

namespace Entity
{
    [Table("Users", Schema = Constants.SCHEMA_NAME)]
    public partial class User : MasterDataEntityBase
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string Password { get; set; }
        public Guid RoleId { get; set; }
        public bool IsChangePassword { get; set; } = false;

        public virtual Role Role { get; set; }
        public virtual IList<Customer> Customers { get; set; }
    }
}
