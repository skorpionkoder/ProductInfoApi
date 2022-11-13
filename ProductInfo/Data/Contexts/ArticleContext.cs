using Microsoft.EntityFrameworkCore;
using ProductInfo.Data.Entities;

namespace ProductInfo.Data.Contexts
{
    public class ArticleContext : DbContext
    {
        public ArticleContext(DbContextOptions<ArticleContext> options) : base(options)
        {
        }
        public DbSet<Article> Articles { get; set; }
    }
}
