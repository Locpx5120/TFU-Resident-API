using System.ComponentModel.DataAnnotations;

namespace Core.Entity
{
    public class EntityBase : IEntityBase
    {
        [Key]
        public Guid Id { get; set; }
        public bool? IsDeleted { get; set; }
        public Guid? InsertedById { get; set; }
        public DateTime? InsertedAt { get; set; }
        public Guid? UpdatedById { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
