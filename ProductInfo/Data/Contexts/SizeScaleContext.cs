using Microsoft.EntityFrameworkCore;
using ProductInfo.Data.Entities;

namespace ProductInfo.Data.Contexts
{
    public class SizeScaleContext : DbContext
    {
        public SizeScaleContext(DbContextOptions<SizeScaleContext> options) : base(options)
        {
        }
        public DbSet<SizeScale> SizeScales { get; set; }
    }
}
