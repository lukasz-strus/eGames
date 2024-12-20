using Application;
using Application.Authentication;
using Application.Authorization;
using FluentValidation;
using Infrastructure;
using Infrastructure.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using NLog.Web;
using Web.Middleware;
using Web.NET;

var builder = WebApplication.CreateBuilder(args);

// AddItem services to the container.

// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

var presentationAssembly = typeof(Presentation.AssemblyReference).Assembly;

builder.Services.AddControllers().AddApplicationPart(presentationAssembly);

builder.Services.AddScoped<ErrorHandlerMiddleware>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "eGamesAPI",
        Version = "v1",
        Description = "API for the eGames platform"
    });

    c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "bearerAuth"
                }
            },
            []
        }
    });
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(PolicyNames.HasDomainUser, b => b.RequireClaim(AppClaimTypes.DomainUserId));

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.BearerScheme;
        options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
        options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
    })
    .AddBearerToken(IdentityConstants.BearerScheme);


builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddClaimsPrincipalFactory<CustomUserClaimsPrincipalFactory>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<ApplicationSeeder>();
    await seeder.Seed();
}

// Configure the HTTP request pipeline.

ValidatorOptions.Global.LanguageManager.Enabled = false;

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});


app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGroup("/api/identity")
    .WithTags("Identity")
    .MapCustomIdentityApi<ApplicationUser>();


app.Run();