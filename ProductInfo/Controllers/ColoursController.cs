using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductInfo.Data.Interfaces;
using ProductInfo.Models;

namespace ProductInfo.Controllers
{
    [ApiController]
    [Route("api/v1/colours")]
    public class ColoursController : ControllerBase
    {
        private readonly IColourInfoRepository _colourInfoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ColoursController> _logger;

        public ColoursController(IColourInfoRepository colourInfoRepository, IMapper mapper, ILogger<ColoursController> logger)
        {
            _colourInfoRepository = colourInfoRepository ?? throw new ArgumentNullException(nameof(colourInfoRepository));
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{colourId}")]
        public async Task<ActionResult<ColourModel>> GetColours(Guid colourId)
        {
            try
            {
                _logger.LogInformation($"HttpGet: GetColours(), Request: ColourId {colourId}");

                var colour = await _colourInfoRepository.GetColoursAsync(colourId);

                if (colour == null) return NotFound();
                
                _logger.LogInformation($"HttpGet: GetColours(), Response: Colour {colour}");

                return _mapper.Map<ColourModel>(colour);
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred when calling Colour API.", ex.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}
