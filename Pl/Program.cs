using System.Text;
using System.Text.Json.Serialization;
using Bll.Extensions;
using Bll.MapperConfiguration;
using Bll.Services;
using Dal.DatabaseContext;
using Dal.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using UpDownCryptorollBackend.Extensions;
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", b =>
    {
        b.WithOrigins(
                "http://172.27.33.20:3000",
                "http://172.27.33.20:5173",
                "http://localhost:3000",
                "https://cryptoroll.su",
                "http://cryptoroll.su",
                Environment.GetEnvironmentVariable("FRONTEND_URL") ?? string.Empty)
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddQuartz();
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = false);

builder.Services.AddBll();
builder.Services.AddPl();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")))
        };
    });

var app = builder.Build();

// Apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();

    var jobScheduler = scope.ServiceProvider.GetRequiredService<JobScheduleService>();
    await jobScheduler.SetUpdatingLivePrice();
}

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseCors("AllowSpecificOrigins");

app.Run();
