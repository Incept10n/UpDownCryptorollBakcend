using System.Runtime.InteropServices.ComTypes;
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

        CreateMap<Match, MatchDto>()
            .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
            .ForMember(dest => dest.Coin, opts => opts.MapFrom(src => src.Coin))
            .ForMember(dest => dest.EntryDateTime, opts => opts.MapFrom(src => src.EntryTime))
            .ForMember(dest => dest.EntryPrice, opts => opts.MapFrom(src => src.EntryPrice))
            .ForMember(dest => dest.ExitDateTIme, opts => opts.MapFrom(src => src.ExitTime))
            .ForMember(dest => dest.ExitPrice, opts => opts.MapFrom(src => src.ExitPrice))
            .ForMember(dest => dest.PredictionValue, opts => opts.MapFrom(src => src.Prediction))
            .ForMember(dest => dest.PredictionTimeframe, opts => opts.MapFrom(src => src.PredictionTimeframe))
            .ForMember(dest => dest.ResultStatus, opts => opts.MapFrom(src => src.Res))
            .ForMember(dest => dest.ResultPayout, opts => opts.MapFrom(src => src.ResultPayout));


        CreateMap<Match, CurrentMatchDto>()
            .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
            .ForMember(dest => dest.Bet, opts => opts.MapFrom(src => src.PredictionAmount))
            .ForMember(dest => dest.Coin, opts => opts.MapFrom(src => src.Coin))
            .ForMember(dest => dest.Prediction, opts => opts.MapFrom(src => src.Prediction))
            .ForMember(dest => dest.TimeRemaining, opts => opts.MapFrom(src =>
                src.EntryTime + src.PredictionTimeframe - DateTimeOffset.Now.ToUniversalTime()
            ));
    }
}
