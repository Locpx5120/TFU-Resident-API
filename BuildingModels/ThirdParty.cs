﻿using Core.Entity;

namespace BuildingModels;

public partial class ThirdParty : MasterDataEntityBase
{
    public string NameCompany { get; set; }
    public string Description { get; set; }
    public bool Status { get; set; }
    public bool IsTenant { get; set; }
    //public Guid? UserId { get; set; }
    public Guid? StaffId { get; set; }
    public virtual Staff Staff { get; set; }
}
