using Bll.Managers;
using Bll.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Bll.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddBll(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<UserService>();
        serviceCollection.AddScoped<MatchService>();
        serviceCollection.AddScoped<CurrentPriceService>();
        serviceCollection.AddScoped<RegexManager>();
        serviceCollection.AddScoped<JobScheduleService>();
        serviceCollection.AddScoped<RewardsService>();
        serviceCollection.AddScoped<RewardTaskService>();
        serviceCollection.AddScoped<JwtTokenManager>();
    }
}