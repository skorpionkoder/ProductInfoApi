using System.ComponentModel.DataAnnotations;

namespace ProductInfo.Models
{
    public class ProductDetailModel
    {
        public Guid ProductId { get; set; }
        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        [Required]
        public Guid SizeScaleId { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }

        [Range(1, 3)]
        public int ChannelId { get; set; }

        [Required]
        public ICollection<ArticleModel> Articles { get; set; }
        [Required]
        public ICollection<SizeScaleModel> Sizes { get; set; }
    }
}
