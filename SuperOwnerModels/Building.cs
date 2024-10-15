using Constant;
using Core.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperOwnerModels
{
    [Table("Buildings", Schema = Constants.SCHEMA_NAME)]
    public class Building : MasterDataEntityBase
    {
        public string Name { get; set; } = null!;
        public string Permalink { get; set; } = null!;
        public string ConnectionString { get; set; } = null!;

        public double MaxNumberApartments { get; set; } = 0; // căn hộ tối đa
        public double MaxNumberResidents { get; set; } = 0; // cư dân tối đa

        public Guid? ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public virtual Project Project { get; set; } = null!;
    }
}
