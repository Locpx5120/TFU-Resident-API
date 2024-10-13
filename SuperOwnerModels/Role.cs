using Constant;
using Core.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    [Table("Roles", Schema = Constants.SCHEMA_NAME)]
    public partial class Role : MasterDataEntityBase
    {
        public string? Name { get; set; }

        public virtual IList<User> Users { get; set; }
    }
}
