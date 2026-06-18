using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using JewelryAPI.Infrastructure;
using JewelryAPI.Application;
using System.Text;

using Swashbuckle.AspNetCore.Filters;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrEmpty(connectionString) && (connectionString.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase) || connectionString.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase)))
{
    var databaseUri = new Uri(connectionString);
    var userInfo = databaseUri.UserInfo.Split(':');
    var parsedConnectionString = $"Host={databaseUri.Host};Port={(databaseUri.Port > 0 ? databaseUri.Port : 5432)};Database={databaseUri.LocalPath.TrimStart('/')};Username={userInfo[0]};Password={(userInfo.Length > 1 ? userInfo[1] : "")};SSL Mode=Require;Trust Server Certificate=true;";
    builder.Configuration["ConnectionStrings:DefaultConnection"] = parsedConnectionString;
}


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
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

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
});

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings.GetValue<string>("Secret") ?? "DefaultSecretKeyNeedToChangeThisBeforeProd";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
        ValidAudience = jwtSettings.GetValue<string>("Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

var app = builder.Build();

app.UseMiddleware<JewelryAPI.Web.Middleware.ExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    await JewelryAPI.Web.Extensions.DbSeeder.SeedAsync(scope.ServiceProvider);
}

// Enable Swagger in all environments (including Render Production)
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Add a simple root endpoint to prevent 404 errors when accessing the base URL
app.MapGet("/", () => "Jewelry API is running successfully");

app.Run();
