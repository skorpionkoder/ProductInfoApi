using Microsoft.EntityFrameworkCore;
using ProductInfo.Data.Entities;

namespace ProductInfo.Data.Contexts
{
    public class ColourContext : DbContext
    {
        public ColourContext(DbContextOptions<ColourContext> options) : base(options)
        {
        }
        public DbSet<Colour> Colours { get; set; }
    }
}
