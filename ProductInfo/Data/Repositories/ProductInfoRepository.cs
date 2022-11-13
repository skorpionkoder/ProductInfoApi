using Microsoft.EntityFrameworkCore;
using ProductInfo.Data.Contexts;
using ProductInfo.Data.Entities;
using ProductInfo.Data.Interfaces;
using ProductInfo.Models;

namespace ProductInfo.Data.Repositories
{
    public class ProductInfoRepository : IProductInfoRepository
    {
        private readonly ProductContext _context;
        private readonly ILogger<ProductInfoRepository> _logger;

        public ProductInfoRepository(ProductContext context, ILogger<ProductInfoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Add<T>(T entity) where T : class
        {
            _logger.LogInformation($"Adding an object of type {entity.GetType()} to the context.");
            _context.Add(entity);
        }

        public async Task<Product> GetProductAsync(Guid productId)
        {
            _logger.LogInformation($"Getting Product with ID {productId}.");

            var product = _context.Products
                .Where(p => p.ProductId == productId);

            return await product.FirstOrDefaultAsync();
        }

        public string GetProductCodeForOnlineChannel(int channelId, int productYear)
        {
            _logger.LogInformation($"Getting Product Code for Channel Id {channelId}.");

            var productCode = _context.Products
                .Where(p => p.ChannelId == channelId && p.ProductYear == productYear)
                .Select(c => c.ProductCode)
                .Max();

            return productCode;
        }

        public string GetProductCodeForAllChannel(int channelId)
        {
            _logger.LogInformation($"Getting Product Code for Channel Id {channelId}.");

             var productCode = _context.Products
                .Where(p => p.ChannelId == channelId)
                .Select(c => c.ProductCode)
                .Max();

            return productCode;
        }

        public bool ProductCodeExists(int channelId, string productCode)
        {
            _logger.LogInformation($"Checking if Product Code {productCode} exists for Channel ID {channelId}.");

            return _context.Products.Any(p => p.ChannelId == channelId && p.ProductCode == productCode);
        }

        public async Task<bool> SaveChangesAsync()
        {
            _logger.LogInformation($"Attempitng to save the changes in the context");

            // Only return success if at least one row was changed
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
