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
            .ForMember(dest => dest.PredictionAmount, opts => opts.MapFrom(src => src.PredictionAmount))
            .ForMember(dest => dest.PredictionTimeframe, opts => opts.MapFrom(src => src.PredictionTimeframe))
            .ForMember(dest => dest.PredictionValue, opts => opts.MapFrom(src => src.PredictionValue));

        CreateMap<RewardStatusDto, RewardStatusModel>()
            .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.UserName))
            .ForMember(dest => dest.LoginStreakCount, opts => opts.MapFrom(src => src.LoginStreakCount))
            .ForMember(dest => dest.LastRewardedTime, opts => opts.MapFrom(src => src.LastRewardedTime))
            .ForMember(dest => dest.LastLoginTime, opts => opts.MapFrom(src => src.LastLoginTime))
            .ForMember(dest => dest.isRewardCollected, opts => opts.MapFrom(src => src.IsRewardCollected));
        
        
        CreateMap<MatchDto, MatchModel>()
            .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
            .ForMember(dest => dest.Coin, opts => opts.MapFrom(src => src.Coin))
            .ForMember(dest => dest.EntryDateTime, opts => opts.MapFrom(src => src.EntryDateTime))
            .ForMember(dest => dest.EntryPrice, opts => opts.MapFrom(src => src.EntryPrice))
            .ForMember(dest => dest.ExitDateTIme, opts => opts.MapFrom(src => src.ExitDateTIme))
            .ForMember(dest => dest.ExitPrice, opts => opts.MapFrom(src => src.ExitPrice))
            .ForMember(dest => dest.PredictionValue, opts => opts.MapFrom(src => src.PredictionValue))
            .ForMember(dest => dest.PredictionTimeframe, opts => opts.MapFrom(src => src.PredictionTimeframe))
            .ForMember(dest => dest.ResultStatus, opts => opts.MapFrom(src => src.ResultStatus))
            .ForMember(dest => dest.ResultPayout, opts => opts.MapFrom(src => src.ResultPayout));
        
        CreateMap<CurrentMatchDto, CurrentMatchModel>()
            .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
            .ForMember(dest => dest.Bet, opts => opts.MapFrom(src => src.Bet))
            .ForMember(dest => dest.Coin, opts => opts.MapFrom(src => src.Coin))
            .ForMember(dest => dest.Prediction, opts => opts.MapFrom(src => src.Prediction))
            .ForMember(dest => dest.TimeRemaining, opts => opts.MapFrom(src => src.TimeRemaining))
            .ForMember(dest => dest.WinningMultiplier, opts => opts.MapFrom(src => src.WinningMultiplier))
            .ForMember(dest => dest.EntryPrice, opts => opts.MapFrom(src => src.EntryPrice));
    }
}