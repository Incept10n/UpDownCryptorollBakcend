using Bll.Managers;
using Bll.Services;
using Dal.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Bll.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddBll(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<UserService>();
        serviceCollection.AddScoped<MatchService>();
        serviceCollection.AddScoped<CurrentPriceService>();
        serviceCollection.AddScoped<RegexManager>();
        serviceCollection.AddScoped<JobScheduleService>();
        serviceCollection.AddScoped<RewardsService>();
        serviceCollection.AddScoped<RewardTaskService>();
        serviceCollection.AddScoped<JwtTokenManager>();
        serviceCollection.AddScoped<ReferralService>();
        serviceCollection.AddScoped<QuizService>();

        return serviceCollection;
    }
}