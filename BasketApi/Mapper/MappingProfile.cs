using AutoMapper;
using BasketApi.Dtos;
using BasketApi.Database.Entities;

namespace BasketApi.Mapper;

internal sealed class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<BasketItem, BasketItem>().ReverseMap();
	}
}
