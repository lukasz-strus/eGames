using MediatR;
using NLog;

namespace Application.Internals.Behaviors;

internal class LoggerBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        TResponse response;
        Logger.Info("Handling {RequestType}", request.GetType().Name);

        try
        {
            response = await next();
        }
        catch (Exception e)
        {
            Logger.Error(e, "Error handling {RequestType}", request.GetType().Name);
            throw;
        }

        Logger.Info("Handled {RequestType}", request.GetType().Name);

        return response;
    }
}