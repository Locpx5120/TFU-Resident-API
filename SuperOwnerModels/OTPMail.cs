using Core.Entity;
using Entity;

namespace TFU_Resident_API.Entity
{
    public class OTPMail : MasterDataEntityBase
    {
        public string Otp { get; set; }
        public string ContentMail { get; set; }
        public string TypeOtp { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
