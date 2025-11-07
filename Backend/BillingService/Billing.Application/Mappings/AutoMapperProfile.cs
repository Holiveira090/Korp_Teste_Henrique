using AutoMapper;
using Billing.Application.DTOs;
using Billing.Domain.Models;

namespace Billing.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Invoice, InvoiceDTO>().ReverseMap();
            CreateMap<InvoiceItem, InvoiceItemDto>().ReverseMap();
        }
    }
}
