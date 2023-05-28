using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Remore.Backend.DAL;
using Remore.Backend.DAL.Models;
using Remore.Backend.Models;
using Remore.Backend.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

var connectionString = builder.Configuration.GetConnectionString("NPGSQL");

services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
services.AddAuthentication(options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "Remore",
            ValidAudience = "Remore",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWTSecret"])
            ),
        };
    });
services.AddIdentityCore<AppUser>(options =>
{
    options.SignIn.RequireConfirmedEmail = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
services.AddHttpContextAccessor();
services.Configure<ApiBehaviorOptions>(opt =>
{
    opt.SuppressModelStateInvalidFilter = false;
    opt.InvalidModelStateResponseFactory = (e) =>
    {
        var errors = e.ModelState.Select(x => x.Value.Errors.Select(x => x.ErrorMessage));
        var errorsFormatted = new List<string>();
        foreach (var error in errors)
        {
            errorsFormatted.AddRange(error);
        }

        return new BadRequestObjectResult(errorsFormatted.AsErrorResponse());
    };
});

services.AddSingleton<IMapper, Mapper>(x =>
{
    return new Mapper(
        new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AppUser, PublicUser>();
            cfg.CreateMap<AppUser, PrivateUser>();
        })
    );
});
services.AddScoped<TokenService>();
services.AddScoped<AuthService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
