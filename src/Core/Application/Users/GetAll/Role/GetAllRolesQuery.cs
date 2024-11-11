using Application.Contracts.User;
using Domain.Core.Results;
using MediatR;

namespace Application.Users.GetAll.Role;

public record GetAllRolesQuery : IRequest<Result<UserRoleListResponse>>;