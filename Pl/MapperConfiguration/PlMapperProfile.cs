using AutoMapper;
using Bll.Dtos;
using Bll.Dtos.Tasks;
using Dal.Entities.User;
using UpDownCryptorollBackend.Models;
using UpDownCryptorollBackend.Models.Tasks;
using UpDownCryptorollBackend.Models.Users;

namespace UpDownCryptorollBackend.MapperConfiguration;

public class PlMapperProfile : Profile
{
    public PlMapperProfile()
    {
        CreateMap<UserDto, UserModel>()
            .ForMember(dest => dest.Username, opts => opts.MapFrom(src => src.Name))
            .ForMember(dest => dest.WalletAddress, opts => opts.MapFrom(src => src.WalletAddress))
            .ForMember(dest => dest.CurrentBalance, opts => opts.MapFrom(src => src.CurrentBalance))
            .ForMember(dest => dest.LoginStreakCount, opts => opts.MapFrom(src => src.LoginStreakCount))
            .ForMember(dest => dest.IsLastMatchCollected, opts => opts.MapFrom(src => src.IsLastMatchCollected));

        CreateMap<UserChangeInfoModel, UserChangeInfoDto>()
            .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
            .ForMember(dest => dest.Password, opts => opts.MapFrom(src => src.Password))
            .ForMember(dest => dest.WalletAddress, opts => opts.MapFrom(src => src.WalletAddress));
        
        CreateMap<UserCreationModel, UserCreationDto>()
            .ForMember(dest => dest.Username, opts => opts.MapFrom(src => src.Username))
            .ForMember(dest => dest.Password, opts => opts.MapFrom(src => src.Password));

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
            .ForMember(dest => dest.Bet, opts => opts.MapFrom(src => src.Bet))
            .ForMember(dest => dest.Coin, opts => opts.MapFrom(src => src.Coin))
            .ForMember(dest => dest.EntryDateTime, opts => opts.MapFrom(src => src.EntryDateTime))
            .ForMember(dest => dest.EntryPrice, opts => opts.MapFrom(src => src.EntryPrice))
            .ForMember(dest => dest.ExitDateTime, opts => opts.MapFrom(src => src.ExitDateTIme))
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

        CreateMap<RewardTaskDto, RewardTaskModel>()
            .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
            .ForMember(dest => dest.Reward, opts => opts.MapFrom(src => src.Reward))
            .ForMember(dest => dest.Status, opts => opts.MapFrom(src => src.Status));

        CreateMap<TaskTypeChangeModel, RewardTaskChangeDto>()
            .ForMember(dest => dest.TaskId, opts => opts.MapFrom(src => src.TaskId))
            .ForMember(dest => dest.ChangedStatus, opts => opts.MapFrom(src => src.ChangedStatus));
    }
}