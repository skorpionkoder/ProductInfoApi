using Microsoft.EntityFrameworkCore;
using ProductInfo.Data.Contexts;
using ProductInfo.Data.Entities;
using ProductInfo.Data.Interfaces;

namespace ProductInfo.Data.Repositories
{
    public class ColourInfoRepository : IColourInfoRepository
    {
        private readonly ColourContext _context;
        private readonly ILogger<ColourInfoRepository> _logger;

        public ColourInfoRepository(ColourContext context, ILogger<ColourInfoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Colour> GetColoursAsync(Guid colourId)
        {
            _logger.LogInformation($"Getting Colour for Colour ID {colourId}.");

            var colour = _context.Colours
                .Where(c => c.Id == colourId);

            return await colour.FirstOrDefaultAsync();
        }
    }
}
