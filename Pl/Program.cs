using System.Text.Json.Serialization;
using Bll.Extensions;
using Bll.MapperConfiguration;
using Bll.Services;
using Dal.Extensions;
using Quartz;
using UpDownCryptorollBackend.Filters;
using UpDownCryptorollBackend.MapperConfiguration;

// TODO: test create match endpoint for low (15sec timeframe and once/twice for big one (30 mins)
// TODO: valid timeframe (one of 3: 30mins, 4hrs, 12hrs) checking and other data validation for MatchCreationModel
// TODO: add bet relative to price checking when trying to create a match (you can't bet more than you have)
// TODO: add player daily login multiplier when calculating result of the match
// TODO: check all other validation of all endpoints

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

DotNetEnv.Env.Load();
var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION");

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomExceptionFilter>();
}).AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddHttpClient();
builder.Services.AddApplicationDbContext(connectionString);
builder.Services.AddAutoMapper(typeof(BllMapperProfile), typeof(PlMapperProfile));

builder.Services.AddQuartz();
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = false);

builder.Services.AddBll();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var jobScheduler = scope.ServiceProvider.GetRequiredService<JobScheduleService>();

    await jobScheduler.SetUpdatingLivePrice();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
