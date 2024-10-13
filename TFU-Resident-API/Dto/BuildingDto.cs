namespace TFU_Resident_API.Dto
{
    public class CreateBuildingDto
    {
        public Guid ProjectId { get; set; }
        public string Permalink { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
