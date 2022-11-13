using System.Collections.Generic;
using System.Collections.ObjectModel;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductInfo.Data.Entities;
using ProductInfo.Data.Interfaces;
using ProductInfo.Data.Repositories;
using ProductInfo.Models;

namespace ProductInfo.Controllers
{
    [ApiController]
    [Route("api/v1/sizescale")]
    public class SizeScalesController : ControllerBase
    {
        private readonly ISizeScaleInfoRepository _sizeScaleInfoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SizeScalesController> _logger;

        public SizeScalesController(ISizeScaleInfoRepository sizeScaleInfoRepository, IMapper mapper, ILogger<SizeScalesController> logger)
        {
            _sizeScaleInfoRepository = sizeScaleInfoRepository ?? throw new ArgumentNullException(nameof(sizeScaleInfoRepository));
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{sizeId}")]
        public async Task<ActionResult<ICollection<SizeScaleModel>>> GetSizeScales(Guid sizeId)
        {
            try
            {
                _logger.LogInformation($"HttpGet: GetSizeScales(), Request: SizeScaleId {sizeId}");

                ICollection<SizeScaleModel> sizes = new Collection<SizeScaleModel>();

                var sizeScales = await _sizeScaleInfoRepository.GetSizeScalesAsync(sizeId);

                if (sizeScales == null) return NotFound();

                foreach (SizeScale size in sizeScales)
                {
                    sizes.Add(_mapper.Map<SizeScaleModel>(size));
                }
                _logger.LogInformation($"HttpGet: GetSizeScales(), Response: SizeScale {sizes}");

                return Ok(sizes);
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred when calling SizeScale API.", ex.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}
