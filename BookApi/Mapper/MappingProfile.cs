using AutoMapper;
using BookApi.Database.Entities;
using BookApi.Dtos;

namespace BookApi.Mapper;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Book, BookDto>().ReverseMap();
    }
}