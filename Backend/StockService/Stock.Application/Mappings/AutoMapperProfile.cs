using AutoMapper;
using Stock.Application.DTOs;
using Stock.Domain.Models;

namespace Stock.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
