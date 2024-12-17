namespace TFU_Building_API.Dto
{
    public class PackageServiceBasicDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal? Discount { get; set; }
        public int DurationInMonth { get; set; }
    }

}
