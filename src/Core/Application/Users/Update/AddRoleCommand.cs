using Domain.Core.Results;
using MediatR;

namespace Application.Users.Update;

public record AddRoleCommand(Guid UserId, int RoleId) : IRequest<Result<Unit>>;