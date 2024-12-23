using Core.Entity;

namespace BuildingModels;

public partial class Building : MasterDataEntityBase
{
    public string? Name { get; set; }
    public int? NumberFloor { get; set; }
    public int? NumberApartment { get; set; }
    public string Address { get; set; }
    public DateTime? CreateAt { get; set; } // thời điểm toà nhà đc tạo
    public Guid? ProjectId { get; set; }

    public virtual ICollection<Notify> Notify { get; set; } = new List<Notify>();

}
