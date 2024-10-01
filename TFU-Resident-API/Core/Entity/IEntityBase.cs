using System.ComponentModel.DataAnnotations;

namespace Core.Entity
{
    public interface IEntityBase
    {
        [Key]
        Guid Id { get; set; }
        bool? IsDeleted { get; set; }
        Guid? InsertedById { get; set; }
        DateTime? InsertedAt { get; set; }
        Guid? UpdatedById { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}
