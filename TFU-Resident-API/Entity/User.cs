using Constant;
using Core.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("Users", Schema = Constants.SCHEMA_NAME)]
    public partial class User : MasterDataEntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string Password { get; set; }
        public Guid RoleId { get; set; }
        public decimal AvailableBalance { get; set; } // số dư khả dụng
        public decimal PromotionalBalance { get; set; } // số dư khuyến mại
    }
}
