using Application.Contracts.User;
using Domain.Core.Results;
using MediatR;

namespace Application.Users.Get;

public record GetUserRolesQuery(Guid UserId) : IRequest<Result<UserRoleListResponse>>;