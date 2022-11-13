using System.ComponentModel.DataAnnotations;

namespace ProductInfo.Data.Entities
{
    public class SizeScale
    {
        [Key]
        public Guid SizeScaleId { get; set; }
        public string SizeName { get; set; }
    }
}
