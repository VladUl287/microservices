using AutoMapper;
using BasketApi.Database.Entities;

namespace BasketApi.Mapper;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BasketItem, BasketItem>().ReverseMap();
    }
}
