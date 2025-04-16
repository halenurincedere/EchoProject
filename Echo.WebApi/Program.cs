using Echo.Data.Contexts;
using Echo.Data.Repositories;
using Echo.Data.UnitOfWork;
using Echo.Business.DataProtection;
using Echo.Business.Operations.User;
using Echo.Business.Operations.Feature;
using Echo.Business.Operations.Settings;
using Echo.Business.Operations.Match;
using Echo.WebApi.Filters;
using Echo.WebApi.Middlewares;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Echo.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

/*───────────────────────────────────────────────────────────────
 * 1. Database – PostgreSQL
 *──────────────────────────────────────────────────────────────*/
var connectionString = builder.Configuration.GetConnectionString("PostgreSql");
builder.Services.AddDbContext<EchoDbContext>(opt => opt.UseNpgsql(connectionString));

/*───────────────────────────────────────────────────────────────
 * 2. Dependency Injection – Repositories, Services, etc.
 *──────────────────────────────────────────────────────────────*/
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IRepository<UserEntity>, Repository<UserEntity>>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Security / Utilities
builder.Services.AddScoped<IDataProtection, DataProtection>();

// Business Services
builder.Services.AddScoped<IUserService,     UserManager>();
builder.Services.AddScoped<IFeatureService,  FeatureManager>();
builder.Services.AddScoped<ISettingService,  SettingManager>();
builder.Services.AddScoped<IMatchService,    MatchManager>();

// Filters
builder.Services.AddScoped<TimeControllerFilter>();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(opt =>
    {
        opt.InvalidModelStateResponseFactory = context =>
        {
            var error = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .FirstOrDefault()?.ErrorMessage ?? "Geçersiz istek.";

            return new BadRequestObjectResult(new { message = error });
        };
    });

/*───────────────────────────────────────────────────────────────
 * 3. Data‑Protection Keys
 *──────────────────────────────────────────────────────────────*/
var keysPath = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "Keys");
Directory.CreateDirectory(keysPath);

builder.Services.AddDataProtection()
      .SetApplicationName("EchoApp")
      .PersistKeysToFileSystem(new DirectoryInfo(keysPath));

/*───────────────────────────────────────────────────────────────
 * 4. JWT Authentication
 *──────────────────────────────────────────────────────────────*/
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer   = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["SecretKey"]!))
        };
    });

/*───────────────────────────────────────────────────────────────
 * 5. Swagger + JWT support
 *──────────────────────────────────────────────────────────────*/
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    var scheme = new OpenApiSecurityScheme
    {
        Scheme        = "Bearer",
        BearerFormat  = "JWT",
        Name          = "JWT Authentication",
        In            = ParameterLocation.Header,
        Type          = SecuritySchemeType.Http,
        Description   = "Put **ONLY** your JWT Bearer Token below!",
        Reference     = new OpenApiReference
        {
            Id   = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    opt.AddSecurityDefinition(scheme.Reference.Id, scheme);
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { scheme, Array.Empty<string>() }
    });
});

var app = builder.Build();

/*───────────────────────────────────────────────────────────────
 * 6. Middleware Pipeline
 *──────────────────────────────────────────────────────────────*/
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalException();
app.UseHttpsRedirection();

app.UseMaintenanceMode(); 
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();