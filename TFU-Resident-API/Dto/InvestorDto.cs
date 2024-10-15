namespace TFU_Resident_API.Dto
{
    public class CreateInvestorDto
    {
        public Guid? UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class ViewManagerResponse
    {
        public double DsCuDan { get; set; } = 0;
        public double DsCanHo { get; set; } = 0;
        public double DsToaNha { get; set; } = 0;
        public double DsDuAn { get; set; } = 0;
    }
}
