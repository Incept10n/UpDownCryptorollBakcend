using System.Text.Json.Serialization;
using Bll.Extensions;
using Bll.MapperConfiguration;
using Bll.Services;
using Dal.Extensions;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using UpDownCryptorollBackend.Filters;
using UpDownCryptorollBackend.MapperConfiguration;

// TODO (FIX): check for current match and dissallow entering another match when there is one in progress 

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

builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
    });

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

app.UseCors("AllowAll");

app.Run();
