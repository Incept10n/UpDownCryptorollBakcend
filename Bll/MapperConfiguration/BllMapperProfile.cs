using AutoMapper;
using Bll.Dtos;
using Dal.Entities;

namespace Bll.MapperConfiguration;

public class BllMapperProfile : Profile
{
    public BllMapperProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.WalletAddress, opts => opts.MapFrom(src => src.WalletAddress))
            .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
            .ForMember(dest => dest.currentBalance, opts => opts.MapFrom(src => src.CurrentBalance))
            .ForMember(dest => dest.LoginStreakCount, opts => opts.MapFrom(src => src.LoginStreakCount));
    }
}
