namespace TFU_Resident_API.Dto
{
    public class CreateProjectDto
    {
        public Guid InvestorId { get; set; }
        public string Name { get; set; } = null!;
        public string Permalink { get; set; } = null!;
    }
}
