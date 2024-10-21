using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Application.Authentication;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddScoped<IUserContext, UserContext>();


        return services;
    }
}