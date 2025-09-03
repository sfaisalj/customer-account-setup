using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ConfigStoreService.Adapters.Data;
using ConfigStoreService.GraphQL.GraphQL;
using ConfigStoreService.Core.Ports.Incoming;
using ConfigStoreService.Core.Ports.Outgoing;
using ConfigStoreService.Adapters.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add Entity Framework
builder.Services.AddDbContext<ConfigStoreDbContext>(options =>
    options.UseInMemoryDatabase("ConfigStoreDb"));

// Add JWT Authentication
var jwtSecret = "your-super-secret-key-that-should-be-in-config-at-least-256-bits-long";
var key = Encoding.ASCII.GetBytes(jwtSecret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Register services with Ports & Adapters pattern
builder.Services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
builder.Services.AddScoped<ConfigStoreService.Core.Ports.Outgoing.IAuthorizationService, ConfigStoreService.Adapters.Services.AuthorizationService>();
builder.Services.AddScoped<IConfigStoreService, ConfigStoreService.Core.Services.ConfigStoreService>();

// Add GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ConfigStoreDbContext>();
    await SeedData.Initialize(context);
}

app.UseAuthentication();

app.MapGraphQL();

app.Run();