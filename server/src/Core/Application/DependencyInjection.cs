using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Application.Authentication;
using FluentValidation;
using MediatR;
using Application.Core.Behaviors;
using Sieve.Services;
using Application.Core.Sieve;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly, includeInternalTypes: true);

        services.AddScoped<IUserContext, UserContext>();
        services.AddHttpContextAccessor();

        services.AddScoped<ISieveProcessor, ApplicationSieveProcessor>();

        return services;
    }
}