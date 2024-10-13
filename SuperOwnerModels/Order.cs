using Constant;
using Core.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperOwnerModels
{
    [Table("Orders", Schema = Constants.SCHEMA_NAME)]
    public class Order : MasterDataEntityBase
    {
        public double? TotalPrice { get; set; }
        public Guid InvestorId { get; set; }

        public virtual Investor Investor { get; set; }

        public virtual IList<OrderDetail> OrderDetails { get; set; }
    }
}
