using AutoMapper;
using ProductInfo.Data.Entities;
using ProductInfo.Models;

namespace ProductInfo.Data.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            this.CreateMap<Product, ProductModel>()
                .ReverseMap();
            this.CreateMap<Product, ProductDetailModel>()
                .ReverseMap();
            this.CreateMap<Colour, ArticleModel>()
                .ReverseMap();
            this.CreateMap<Colour, ColourModel>()
                .ReverseMap();
            this.CreateMap<Article, ArticleModel>()
                .ReverseMap();
            this.CreateMap<SizeScale, SizeScaleModel>()
                .ReverseMap();
        }
    }
}
