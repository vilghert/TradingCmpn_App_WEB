using AutoMapper;
using TradingCompany_WEB.Models;

namespace TradingCompany_WEB.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductDto, ProductDto>().ReverseMap();
        }
    }
}
