using Cefalo.InfedgeBlog.Database.Context;
using Cefalo.InfedgeBlog.Repository.Interfaces;
using Cefalo.InfedgeBlog.Repository.Repositories;
using Cefalo.InfedgeBlog.Service.Dtos.Validators;
using Cefalo.InfedgeBlog.Service.Dtos;
using Cefalo.InfedgeBlog.Service.Interfaces;
using Cefalo.InfedgeBlog.Service.Services;
using Cefalo.InfedgeBlog.Service.Utils;
using Cefalo.InfedgeBlog.Api.Utils.GlobalErrorHandler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);

builder.Services.AddControllers().AddXmlSerializerFormatters();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddScoped<DtoValidatorBase<LoginDto>, LoginDtoValidator>();
builder.Services.AddScoped<DtoValidatorBase<SignupDto>, SignupDtoValidator>();
builder.Services.AddScoped<DtoValidatorBase<StoryPostDto>, StoryPostDtoValidator>();
builder.Services.AddScoped<DtoValidatorBase<StoryUpdateDto>, StoryUpdateDtoValidator>();
builder.Services.AddScoped<DtoValidatorBase<UserPostDto>, UserPostDtoValidator>();
builder.Services.AddScoped<DtoValidatorBase<UserUpdateDto>, UserUpdateDtoValidator>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IStoryRepository, StoryRepository>();
builder.Services.AddScoped<IStoryService, StoryService>();

builder.Services.AddScoped<IJwtTokenHandler, JwtTokenHandler> ();
builder.Services.AddScoped<IDateTimeHandler, DateTimeHandler>();
builder.Services.AddScoped<IPasswordHandler, PasswordHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
