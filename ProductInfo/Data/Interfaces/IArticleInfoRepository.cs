using ProductInfo.Data.Entities;

namespace ProductInfo.Data.Interfaces
{
    public interface IArticleInfoRepository
    {
        Task<ICollection<Article>> GetArticlesAsync(Guid productId);
    }
}
