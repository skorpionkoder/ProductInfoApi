using System.ComponentModel.DataAnnotations;

namespace ProductInfo.Models
{
    public class SizeScaleModel
    {
        [Required]
        public Guid SizeScaleId { get; set; }
        public string SizeName { get; set; }
    }
}
