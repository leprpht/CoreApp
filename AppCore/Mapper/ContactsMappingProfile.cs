using AutoMapper;
using AppCore.Entities;
using AppCore.Dto;

namespace AppCore.Mapper;

public class ContactsMappingProfile : Profile
{
    public ContactsMappingProfile()
    {
        CreateMap<Person, PersonDto>();
        
        CreateMap<CreatePersonDto, Person>();
        
        CreateMap<UpdatePersonDto, Person>();
        
        CreateMap<Address, AddressDto>().ReverseMap();
    }
}