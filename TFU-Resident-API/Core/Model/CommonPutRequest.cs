using System.ComponentModel.DataAnnotations;

namespace Core.Model
{
    public class CommonPutRequest
    {
        [Required]
        public Guid Id { get; set; }
    }
}
