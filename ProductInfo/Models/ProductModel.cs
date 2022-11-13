using System.ComponentModel.DataAnnotations;

namespace ProductInfo.Models
{
    public class ProductModel
    {
        [Required]
        public string ProductName { get; set; }

        [Required]
        public int ProductYear { get; set; }
        [Range(1,3)]
        public int ChannelId { get; set; }
        [Required]
        public Guid SizeScaleId { get; set; }

        public ICollection<ArticleModel> Articles { get; set; }
    }
}
