﻿using Core.Entity;

namespace BuildingModels
{
    public class OTPMail : MasterDataEntityBase
    {
        public string Otp { get; set; }
        public string ContentMail { get; set; }
        public string TypeOtp { get; set; }
        public Guid? ResidentId { get; set; }
        public User? User { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
