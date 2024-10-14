using Constant;
using Core.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperOwnerModels
{
    [Table("Projects", Schema = Constants.SCHEMA_NAME)]
    public class Project : MasterDataEntityBase
    {
        public Project()
        {
            Buildings = new HashSet<Building>();
        }
        public string Name { get; set; } = null!;
        public string Position { get; set; } = null!;
        public string Permalink { get; set; } = null!;
        public Guid? InvestorId { get; set; }
        [ForeignKey(nameof(InvestorId))]
        public virtual Investor Investor { get; set; } = null!;
        public virtual ICollection<Building> Buildings { get; set; }
    }
}
