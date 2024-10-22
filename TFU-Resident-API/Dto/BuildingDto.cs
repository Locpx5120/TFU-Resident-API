namespace TFU_Resident_API.Dto
{
    public class CreateBuildingDto
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = null!;
    }

    public class ViewManagerBuildingResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public double MaxNumberApartments { get; set; } = 0; // căn hộ tối đa
        public double MaxNumberResidents { get; set; } = 0; // cư dân tối đa
    }

    public class ViewManagerBuildingRequest
    {
        public string? Name { get; set; } = null!;
    }

    public class UpdateBuildingRequest
    {

    }

    public class DeleteBuildingRequest
    {

    }

}
