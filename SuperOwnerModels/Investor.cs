using Constant;
using Core.Entity;
using Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperOwnerModels
{
    [Table("Investors", Schema = Constants.SCHEMA_NAME)]
    public class Investor : MasterDataEntityBase
    {
        public Investor()
        {
            Projects = new HashSet<Project>();
        }
        public Guid? UserId { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Project> Projects { get; set; }
    }
}
