using System.Text.Json;
using System.Text.Json.Serialization;
using Bll.Extensions;
using Bll.MapperConfiguration;
using Dal.Extensions;
using Quartz;
using UpDownCryptorollBackend.Filters;
using UpDownCryptorollBackend.MapperConfiguration;

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

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
