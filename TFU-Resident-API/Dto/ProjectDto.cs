using System.ComponentModel.DataAnnotations;

namespace TFU_Resident_API.Dto
{
    public class CreateProjectDto
    {
        [Required]
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
    }

    public class UpdateProjectRequest
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
    }

    public class DeleteProjectRequest
    {
        [Required]
        public Guid Id { get; set; }
    }

    public class ViewManagerProjectResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public double SoToaNha { get; set; } = 0;
        public double SoCanHo { get; set; } = 0;
        public double SlCuDan { get; set; } = 0;
    }

    public class ViewManagerProjectRequest
    {
        public string Name { get; set; } = null!;
    }
}
