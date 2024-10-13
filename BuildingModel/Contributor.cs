using Core.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildingModels;

public partial class Contributor : MasterDataEntityBase
{
    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public Guid? StaffId { get; set; }
    public Guid? ThirdPartyId { get; set; }


    [ForeignKey(nameof(StaffId))]
    public virtual Staff Staff { get; set; } = null!;

    [ForeignKey(nameof(ThirdPartyId))]
    public virtual ThirdParty ThirdParty { get; set; } = null!;
}
