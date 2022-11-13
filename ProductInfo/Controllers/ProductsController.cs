using System.Net.Http.Headers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductInfo.Data.Entities;
using ProductInfo.Data.Interfaces;
using ProductInfo.Models;

namespace ProductInfo.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductInfoRepository _productInfoRepository;
        private readonly IArticleInfoRepository _articleInfoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductsController(IProductInfoRepository productInfoRepository, IArticleInfoRepository articleInfoRepository, IMapper mapper, ILogger<ProductsController> logger)
        {
            _productInfoRepository = productInfoRepository ?? throw new ArgumentNullException(nameof(productInfoRepository));
            _articleInfoRepository = articleInfoRepository ?? throw new ArgumentNullException(nameof(articleInfoRepository));
            _mapper = mapper;
            _logger = logger;
        }

        // Mock purpose
        //public ProductsController(IProductInfoRepository productInfoRepository, IArticleInfoRepository articleInfoRepository, IMapper mapper, ILogger<ProductsController> logger, IHttpClientFactory httpClientFactory) : this(productInfoRepository, articleInfoRepository, mapper, logger)
        //{
        //    _httpClientFactory = httpClientFactory;
        //}

        [HttpGet("{productId}")]
        public async Task<ActionResult<ProductDetailModel>> GetProduct(Guid productId)
        {
            try
            {
                _logger.LogInformation($"HttpGet: GetProduct(), Request: ProductID {productId}");

                var product = await _productInfoRepository.GetProductAsync(productId);

                if (product == null) return NotFound();

                // Get Articles
                var articles = await _articleInfoRepository.GetArticlesAsync(productId);
                product.Articles = articles;

                foreach(Article article in articles)
                {
                    // Get Colour Details
                    var colour = GetColourByColourId(article.ColourId);
                    article.ColourCode = colour.Result.ColourCode;
                    article.ColourName = colour.Result.ColourName;
                }

                // Get Size Scale Details
                product.Sizes = await GetSizeScalesByIdAsync(product.SizeScaleId);

                _logger.LogInformation($"HttpGet: GetProduct(), Response: Product {product}");

                return _mapper.Map<ProductDetailModel>(product); ;
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while getting Product details.", ex.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        private async Task<ICollection<SizeScale>> GetSizeScalesByIdAsync(Guid sizeScaleId)
        {
            ICollection<SizeScale> sizeScales = null;
            string baseUrl = "https://localhost:7064/";

            try
            {
                _logger.LogInformation($"Calling Size Scale API with ID {sizeScaleId}.");

                using (var client = new HttpClient())
                {
                    //Passing service base url  
                    client.BaseAddress = new Uri(baseUrl);

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetDepartments using HttpClient  
                    HttpResponseMessage response = await client.GetAsync($"api/v1/sizescale/{sizeScaleId}");

                    //Checking the response is successful or not which is sent using HttpClient  
                    if (response.IsSuccessStatusCode)
                    {
                        var objResponse = response.Content.ReadAsStringAsync().Result;
                        sizeScales = JsonConvert.DeserializeObject<List<SizeScale>>(objResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while getting Size scale details.", ex.Message);
                throw;
            }
            
            return sizeScales;
        }

        [HttpPost]
        public async Task<ActionResult<ProductModel>> Post(ProductModel model)
        {
            try
            {
                // Return 406 if product name length is greater than 100
                if (model.ProductName.Length > 100) return this.StatusCode(StatusCodes.Status406NotAcceptable, "The field ProductName must be a string or array type with a maximum length of '100'.");

                _logger.LogInformation($"HttpPost Request: Product {model}");

                // Create a new Product
                var product = _mapper.Map<Product>(model);
                product.ProductId = Guid.NewGuid();

                product.CreateDate = DateTime.Now;
                Request.Headers.TryGetValue("Host", out var createdHost);
                product.CreatedBy = createdHost;

                // Create Product Code
                var productCode = CreateProductCode(product.ChannelId, product.ProductYear);
                product.ProductCode = productCode;

                // Create Article
                if(await CreateArticlesAsync(product))
                {
                    _logger.LogInformation($"Created articles for Product - {product.ProductName}.");
                }

                _productInfoRepository.Add(product);
                if (await _productInfoRepository.SaveChangesAsync())
                {
                    _logger.LogInformation($"HttpPost Response: Product {product.ProductId}");
                    return Created($"api/products/{product.ProductId}", _mapper.Map<ProductModel>(product));
                }
            }
            catch(ApplicationException ex)
            {
                _logger.LogError("An exception occurred while creating Product Code.", ex.Message);
                return this.StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while creating Product details.", ex.Message);
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();
        }

        private async Task<bool> CreateArticlesAsync(Product product)
        {
            try
            {
                foreach (Article article in product.Articles)
                {
                    // Get Colour Code
                    var colour = GetColourByColourId(article.ColourId);

                    // Create Article
                    article.ArticleId = Guid.NewGuid();
                    article.ArticleName = $"{product.ProductName} - {colour.Result.ColourCode}";
                    article.ProductId = product.ProductId;
                    article.ColourCode = colour.Result.ColourCode;
                    article.ColourName = colour.Result.ColourName;

                    _productInfoRepository.Add(article);
                    if (await _productInfoRepository.SaveChangesAsync())
                    {
                        //log message
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while creating Article details.", ex.Message);
                throw;
            }
        }

        private async Task<Colour> GetColourByColourId(Guid id)
        {
            Colour colour = null;
            string baseUrl = "https://localhost:7064/";
            // testing iis
            //string baseUrl = "https://localhost:44321/";

            try
            {
                _logger.LogInformation($"Calling Colours API with ID {id}.");

                using (var client = new HttpClient())
                {
                    //Passing service base url  
                    client.BaseAddress = new Uri(baseUrl);

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Sending request to find web api REST service resource GetDepartments using HttpClient  
                    HttpResponseMessage response = await client.GetAsync($"api/v1/colours/{id}");

                    //Checking the response is successful or not which is sent using HttpClient  
                    if (response.IsSuccessStatusCode)
                    {
                        var objResponse = response.Content.ReadAsStringAsync().Result;
                        colour = JsonConvert.DeserializeObject<Colour>(objResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An exception occurred while getting Colour details.", ex.Message);
                throw;
            }
            
            return colour;
        }

        private string CreateProductCode(int channelId, int productYear)
        {
            string productCode = String.Empty;

            switch (channelId)
            {
                case 1:
                    var productCodeOnline = _productInfoRepository.GetProductCodeForOnlineChannel(channelId, productYear);
                    productCode = string.IsNullOrEmpty(productCodeOnline) ? $"{productYear}001" : $"{Convert.ToInt32(productCodeOnline) + 1}";
                    break;

                case 2:
                    productCode = GenerateRandomString(6, channelId);
                    break;

                case 3:
                    var productCodeAll = _productInfoRepository.GetProductCodeForAllChannel(channelId);
                    productCode = string.IsNullOrEmpty(productCodeAll) ? $"10000000" : $"{Convert.ToInt32(productCodeAll) + 1}";
                    break;

                default:
                    throw new ApplicationException("Invalid Channel ID");
            }

            return productCode;
        }

        private string GenerateRandomString(int length, int channelId)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string res = String.Empty;
            do
            {
                res = new string(Enumerable.Repeat(chars, length)
                        .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            while (_productInfoRepository.ProductCodeExists(channelId, res));
            return res;
        }
    }
}
