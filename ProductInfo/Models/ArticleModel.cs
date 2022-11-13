using System.ComponentModel.DataAnnotations;

namespace ProductInfo.Models
{
    public class ArticleModel
    {
        public Guid ArticleId { get; set; }
        public string? ArticleName { get; set; }
        [Required]
        public Guid ColourId { get; set; }
        public string? ColourCode { get; set; }
        public string? ColourName { get; set; }
    }
}
