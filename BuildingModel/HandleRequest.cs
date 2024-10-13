using Core.Entity;

namespace BuildingModels;

public partial class HandleRequest : MasterDataEntityBase
{
    public Guid RequestionId { get; set; }
    public Guid? AssigmentId { get; set; }
    public virtual Assigment? Assigment { get; set; }
    public virtual RequestComplain Requestion { get; set; } = null!;
}
