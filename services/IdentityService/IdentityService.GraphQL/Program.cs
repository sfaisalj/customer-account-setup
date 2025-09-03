using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using IdentityService.Adapters.Data;
using IdentityService.GraphQL.GraphQL;
using IdentityService.Core.Ports.Incoming;
using IdentityService.Core.Ports.Outgoing;
using IdentityService.Core.Services;
using IdentityService.Adapters.Repositories;
using IdentityService.Adapters.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Entity Framework
builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseInMemoryDatabase("IdentityDb"));

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
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenService>(provider => new JwtTokenService(jwtSecret));
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IdentityService.Core.Ports.Outgoing.IAuthorizationService, IdentityService.Adapters.Services.AuthorizationService>();

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
    var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
    await SeedData.Initialize(context);
}

app.UseAuthentication();

app.MapGraphQL();

app.Run();