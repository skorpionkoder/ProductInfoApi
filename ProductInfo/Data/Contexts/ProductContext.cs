using Microsoft.EntityFrameworkCore;
using ProductInfo.Data.Entities;

namespace ProductInfo.Data.Contexts
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Article> Articles { get; set; }
    }
}
