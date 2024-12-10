using Core.Entity;
using System.ComponentModel.DataAnnotations;

namespace BuildingModels;

public partial class Notify : MasterDataEntityBase
{
    public string Title { get; set; }  //Tiêu đề
    public DateTime? ApplyDate { get; set; }  //Ngày áp dụng
    public string NotificationType { get; set; } //Loại thông báo
    [MaxLength(int.MaxValue)]
    public string ShortContent { get; set; }
    [MaxLength(int.MaxValue)]
    public string LongContent { get; set; }
    public string Status { get; set; } //Trạng thái
    public string? NoteReject { get; set; } = string.Empty; //Note từ chối

    public Guid BuildingId { get; set; }
    public virtual Building Building { get; set; }
    public Guid? RoleId { get; set; }
    public virtual Role Role { get; set; }
    public Guid? ImgBaseId { get; set; } // link ảnh
    public virtual ImgBase? ImgBase { get; set; }
    public Guid? UserAccpectId { get; set; } = Guid.Empty;
}
