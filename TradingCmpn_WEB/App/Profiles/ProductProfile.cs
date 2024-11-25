using AutoMapper;
using TradingCmpn_WEB.Models;

namespace TradingCmpn_WEB.App.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductDto, ProductModel>();
            CreateMap<ProductModel, ProductDto>();
        }
    }
}
