using Microsoft.EntityFrameworkCore;
using ProductInfo.Data.Contexts;
using ProductInfo.Data.Entities;
using ProductInfo.Data.Interfaces;

namespace ProductInfo.Data.Repositories
{
    public class SizeScaleInfoRepository : ISizeScaleInfoRepository
    {
        private readonly SizeScaleContext _context;
        private readonly ILogger<SizeScaleInfoRepository> _logger;

        public SizeScaleInfoRepository(SizeScaleContext context, ILogger<SizeScaleInfoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ICollection<SizeScale>> GetSizeScalesAsync(Guid sizeId)
        {
            _logger.LogInformation($"Getting Size Scales for Size ID {sizeId}.");

            var size = _context.SizeScales
                .Where(s => s.SizeScaleId == sizeId);

            return await size.ToListAsync();
        }
    }
}
