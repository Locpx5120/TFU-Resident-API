namespace TFU_Building_API.Dto
{
    public class ImageDto
    {
        public Guid Id { get; set; }
        public string Base64 { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string ContentDisposition { get; set; }
        public long Length { get; set; }
    }
}
