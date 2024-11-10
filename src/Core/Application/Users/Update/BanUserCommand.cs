using Domain.Core.Results;
using MediatR;

namespace Application.Users.Update;

public record BanUserCommand(Guid UserId) : IRequest<Result<Unit>>;