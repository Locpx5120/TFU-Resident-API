using Constant;
using Core.Entity;
using Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFU_Resident_API.Entity
{
    [Table("Customer", Schema = Constants.SCHEMA_NAME)]
    public class Customer : MasterDataEntityBase
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public string CodePostion { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
