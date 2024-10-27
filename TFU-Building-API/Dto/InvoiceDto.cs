using System.ComponentModel.DataAnnotations;

namespace TFU_Building_API.Dto
{
    public class CreateInvoiceRequestDto
    {
        [Required]
        public Guid ApartmentId { get; set; }
    }
}
