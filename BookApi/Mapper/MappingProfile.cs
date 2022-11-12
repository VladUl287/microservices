using AutoMapper;
using BookApi.Database.Entities;
using BookApi.Dtos;

namespace BookApi.Mapper;

internal sealed class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<Book, BookDto>().ReverseMap();
	}
}