using Core.Entity;
using System.ComponentModel.DataAnnotations;

namespace BuildingModels
{
    public class Transaction : MasterDataEntityBase
    {
        public string Type { get; set; } // Thể loại giao dịch
        public Guid? TransactionMapId { get; set; } // id của giao dịch 
        public decimal? Price { get; set; } // số tiền
        public string Content { get; set; } // Nội dung giao dịch
        public string Bank { get; set; } // ngân hàng
        public string AccountNumber { get; set; } // tài khoản nhận tiền

        [MaxLength(int.MaxValue)]
        public string RequestBody { get; set; } // nội dung json
        public string Status { get; set; }

    }
}
