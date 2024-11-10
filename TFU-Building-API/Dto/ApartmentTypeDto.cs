using System.ComponentModel.DataAnnotations;

namespace TFU_Building_API.Dto
{
    public class ApartmentResponseTypeDto
    {
        public string Name { get; set; }
        public decimal LandArea { get; set; }
    }

    public class ApartmentTypeDto
    {
        public string Name { get; set; }
        public decimal LandArea { get; set; }
    }
}
