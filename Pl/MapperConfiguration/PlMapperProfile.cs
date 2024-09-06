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

        CreateMap<MatchCreationModel, MatchCreationDto>()
            .ForMember(dest => dest.WalletAddress, opts => opts.MapFrom(src => src.WalletAddress))
            .ForMember(dest => dest.Coin, opts => opts.MapFrom(src => src.Coin))
            .ForMember(dest => dest.EntryPrice, opts => opts.MapFrom(src => src.EntryPrice))
            .ForMember(dest => dest.PredictionAmount, opts => opts.MapFrom(src => src.PredictionAmount))
            .ForMember(dest => dest.PredictionTimeframe, opts => opts.MapFrom(src => src.PredictionTimeframe))
            .ForMember(dest => dest.PredictionValue, opts => opts.MapFrom(src => src.PredictionValue));
    }
}