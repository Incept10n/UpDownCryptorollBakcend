using AutoMapper;
using Bll.Dtos;
using UpDownCryptorollBackend.Models;

namespace UpDownCryptorollBackend.MapperConfiguration;

public class PlMapperProfile : Profile
{
    public PlMapperProfile()
    {
        CreateMap<UserDto, UserModel>()
            .ForMember(dest => dest.Username, opts => opts.MapFrom(src => src.Name))
            .ForMember(dest => dest.CurrentBalance, opts => opts.MapFrom(src => src.currentBalance));

        CreateMap<UserChangeNameModel, UserChangeNameDto>()
            .ForMember(dest => dest.newName, opts => opts.MapFrom(src => src.newName));
    }
}