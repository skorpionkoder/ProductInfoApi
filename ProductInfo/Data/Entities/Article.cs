using System.ComponentModel.DataAnnotations;

namespace ProductInfo.Data.Entities
{
    public class Article
    {
        [Key]
        public Guid ArticleId { get; set; }
        public string ArticleName { get; set; }
        public Guid ColourId { get; set; }
        public string? ColourCode { get; set; }
        public string? ColourName { get; set; }
        public Guid ProductId { get; set; }
    }
}
