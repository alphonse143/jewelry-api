using AutoMapper;
using JewelryAPI.Core.Entities;
using JewelryAPI.Application.DTOs;

namespace JewelryAPI.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<CreateCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>();

        CreateMap<Purchase, PurchaseDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.CustomerName : string.Empty));
        CreateMap<CreatePurchaseDto, Purchase>();
        CreateMap<UpdatePurchaseDto, Purchase>();
    }
}
