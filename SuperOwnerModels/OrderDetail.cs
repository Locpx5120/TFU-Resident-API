using Constant;
using Core.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperOwnerModels
{
    [Table("OrderDetails", Schema = Constants.SCHEMA_NAME)]
    public class OrderDetail : MasterDataEntityBase
    {
        public Guid InvestorId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProjectId {  get; set; }

        public virtual Investor Investor { get; set; }
        public virtual Order Order { get; set; }
        public virtual Project Project { get; set; }
    }
}
