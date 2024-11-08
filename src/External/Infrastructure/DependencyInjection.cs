using Application.Core.Abstractions.Data;
using Domain.Games;
using Domain.Libraries;
using Domain.Orders;
using Domain.Users;
using Infrastructure.Repositories;
using Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("eGames")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<ILibraryRepository, LibraryRepository>();

        services.AddScoped<ApplicationSeeder>();

        return services;
    }
}