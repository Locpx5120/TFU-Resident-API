namespace Core.Entity
{
    public class MasterDataEntityBase : EntityBase, IMasterDataEntityBase
    {
        public MasterDataEntityBase()
        {
            IsActive = true;
        }

        public bool IsActive { get; set; }
    }
}
