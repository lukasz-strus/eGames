using Application.Contracts.User;
using Domain.Core.Results;
using MediatR;

namespace Application.Users.GetAll;

public record GetAllUsersQuery : IRequest<Result<UserListResponse>>;