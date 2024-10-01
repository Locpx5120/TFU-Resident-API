namespace Core.Entity
{
    public interface IMasterDataEntityBase : IEntityBase
    {
        bool IsActive { get; set; }
    }
}
