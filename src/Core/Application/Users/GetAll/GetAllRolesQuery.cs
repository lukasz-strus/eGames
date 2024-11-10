using Application.Contracts.User;
using Domain.Core.Results;
using MediatR;

namespace Application.Users.GetAll;

public record GetAllRolesQuery : IRequest<Result<UserRoleListResponse>>;