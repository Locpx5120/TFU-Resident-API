using Core.Entity;

namespace BuildingModels
{
    public class ImgBase : MasterDataEntityBase
    {
        public string Base64 { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string ContentDisposition { get; set; }
        public long Length { get; set; }

        public virtual ICollection<Notify> Notify { get; set; } = new List<Notify>();
    }
}
