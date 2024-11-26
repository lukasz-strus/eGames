using Application.Contracts.User;
using Domain.Core.Results;
using MediatR;

namespace Application.Users.GetAll.User;

public record GetAllUsersQuery : IRequest<Result<UserListResponse>>;