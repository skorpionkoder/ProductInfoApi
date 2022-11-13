using System.ComponentModel.DataAnnotations;

namespace ProductInfo.Data.Entities
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int ProductYear { get; set; }
        public int ChannelId { get; set; }
        public Guid SizeScaleId { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public ICollection<Article> Articles { get; set; }
        public ICollection<SizeScale> Sizes { get; set; }
    }
}
