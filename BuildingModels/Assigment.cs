using Core.Entity;

namespace BuildingModels;

public partial class Assigment : MasterDataEntityBase
{
    public DateTime? StartTime { get; set; }

    public double? ServicePrice { get; set; }

    public double? ServiceFee { get; set; }

    public DateTime? EndTime { get; set; }
    public Guid? TaskId { get; set; }

    public virtual Task? Task { get; set; }
    public Guid? StaffId { get; set; }
    public virtual Staff? Staff { get; set; }
    public Guid? ServiceContractId { get; set; }
    public virtual ServiceContract? ServiceContract { get; set; }
}
