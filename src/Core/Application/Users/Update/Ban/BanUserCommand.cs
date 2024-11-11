using Domain.Core.Results;
using MediatR;

namespace Application.Users.Update.Ban;

public record BanUserCommand(Guid UserId) : IRequest<Result<Unit>>;