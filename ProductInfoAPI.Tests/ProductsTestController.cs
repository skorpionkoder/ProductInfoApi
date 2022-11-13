using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using ProductInfo.Controllers;
using ProductInfo.Data.Entities;
using ProductInfo.Data.Interfaces;
using ProductInfo.Models;

namespace ProductInfoAPI.Tests
{
    public class ProductsTestController
    {
        private readonly Mock<IProductInfoRepository> productService;
        private readonly Mock<IArticleInfoRepository> articleService;
        private readonly Mock<IColourInfoRepository> colourService;
        private readonly Mock<ISizeScaleInfoRepository> sizeService;
        private readonly Mock<ILogger<ProductsController>> mockLogger;
        private readonly Mock<IMapper> mockMapper;
        //private readonly Mock<IHttpClientFactory> mockHttpClientFactory;
        //private readonly Mock<HttpMessageHandler> mockHttpMessageHandler;

        public ProductsTestController()
        {
            productService = new Mock<IProductInfoRepository>();
            articleService = new Mock<IArticleInfoRepository>();
            colourService = new Mock<IColourInfoRepository>();
            sizeService = new Mock<ISizeScaleInfoRepository>();
            mockLogger = new Mock<ILogger<ProductsController>>();
            mockMapper = new Mock<IMapper>();
            //mockHttpClientFactory = new Mock<IHttpClientFactory>();
            //mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        }

        [Fact]
        public async Task GetProductByIdTest()
        {
            //arrange
            var productList = GetProductsData();

            //mockHttpMessageHandler.Protected()
            //    .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            //    .ReturnsAsync(new HttpResponseMessage
            //    {
            //        StatusCode = HttpStatusCode.OK,
            //        Content = new StringContent("{\"id\":\"d6242a03-5ff5-4976-a446-0e559ed9af25\",\"colourCode\":\"2#9b4703\",\"colourName\":\"2Oregon\"}")
            //    });
            //var client = new HttpClient(mockHttpMessageHandler.Object);
            //mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);


            productService.Setup(x => x.GetProductAsync(productList[0].ProductId))
                .ReturnsAsync(productList[0]);
            articleService.Setup(x => x.GetArticlesAsync(productList[0].ProductId))
                .ReturnsAsync(productList[0].Articles);
            colourService.Setup(x => x.GetColoursAsync(new Guid("d6242a03-5ff5-4976-a446-0e559ed9af25")))
                .ReturnsAsync(new Colour()
                {
                    Id = new Guid("d6242a03-5ff5-4976-a446-0e559ed9af25"),
                    ColourCode = "2#9b4703",
                    ColourName = "2Oregon"
                });
            sizeService.Setup(x => x.GetSizeScalesAsync(new Guid("d977c8a6-105c-420c-a035-632bdbd0ecc7")))
                .ReturnsAsync(new List<SizeScale>
                {
                    new SizeScale
                    {
                        SizeScaleId = new Guid("d977c8a6-105c-420c-a035-632bdbd0ecc7"),
                        SizeName = "Extra Large"
                    }
                });

            var productController = new ProductsController(productService.Object, articleService.Object, mockMapper.Object, mockLogger.Object);

            //act
            var productResult = await productController.GetProduct(productList[0].ProductId);

            //assert
            Assert.NotNull(productResult);
        }

        [Fact]
        public async Task CreateProductTest()
        {
            //arrange
            ProductModel model = new ProductModel
            {
                ProductName = "Puffer jacket",
                ProductYear = 2022,
                ChannelId = 3,
                SizeScaleId = new Guid("d977c8a6-105c-420c-a035-632bdbd0ecc7"),
                Articles = new List<ArticleModel>
                {
                    new ArticleModel
                    {
                        ColourId = new Guid("d6242a03-5ff5-4976-a446-0e559ed9af25")
                    },
                    new ArticleModel
                    {
                        ColourId = new Guid("cdd81814-85df-4958-aee5-0f1d413a5358")
                    }
                }
            };

            var productController = new ProductsController(productService.Object, articleService.Object, mockMapper.Object, mockLogger.Object);

            //act
            var productResult = await productController.Post(model);

            //assert
            Assert.NotNull(productResult);
        }

        private List<Product> GetProductsData()
        {
            List<Product> productsData = new List<Product>
            {
                new Product
                {
                    ProductId = new Guid("50322b69-e10c-423b-9e3e-e323ed299566"),
                    ProductName = "Puffer jacket",
                    ProductCode = "10000001",
                    ProductYear = 2020,
                    ChannelId = 1,
                    SizeScaleId = new Guid("d977c8a6-105c-420c-a035-632bdbd0ecc7"),
                    CreateDate = DateTime.Now,
                    CreatedBy = "localhost",
                    Articles = new List<Article>
                    {
                        new Article
                        {
                            ArticleId = new Guid("e4429425-2e40-4b2d-a776-3fb2397997d9"),
                            ArticleName = "Puffer jacket - 2#9b4703",
                            ColourId = new Guid("d6242a03-5ff5-4976-a446-0e559ed9af25"),
                            ColourCode = "2#9b4703",
                            ColourName = "2Oregon",
                            ProductId = new Guid("50322b69-e10c-423b-9e3e-e323ed299566")
                        },
                        new Article
                        {
                            ArticleId = new Guid("cde40f2a-ccd8-4063-90b4-fd879fb96496"),
                            ArticleName = "Puffer jacket - 1#ab92b3",
                            ColourId = new Guid("cdd81814-85df-4958-aee5-0f1d413a5358"),
                            ColourCode = "1#ab92b3",
                            ColourName = "1Glossy Grape",
                            ProductId = new Guid("50322b69-e10c-423b-9e3e-e323ed299566")
                        }
                    },
                    Sizes = new List<SizeScale>
                    {
                        new SizeScale
                        {
                            SizeScaleId = Guid.NewGuid(),
                            SizeName = "Extra Large"
                        }
                    }
                }
            };

            return productsData;
        }
    }
}