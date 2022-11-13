using Microsoft.EntityFrameworkCore;
using ProductInfo.Data.Contexts;
using ProductInfo.Data.Entities;
using ProductInfo.Data.Interfaces;

namespace ProductInfo.Data.Repositories
{
    public class ArticleInfoRepository : IArticleInfoRepository
    {
        private readonly ArticleContext _context;
        private readonly ILogger<ArticleInfoRepository> _logger;

        public ArticleInfoRepository(ArticleContext context, ILogger<ArticleInfoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ICollection<Article>> GetArticlesAsync(Guid productId)
        {
            _logger.LogInformation($"Getting Articles for {productId}.");

            var articles = _context.Articles
                .Where(a => a.ProductId == productId);

            return await articles.ToListAsync();
        }
    }
}
