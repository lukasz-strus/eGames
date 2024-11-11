using Domain.Core.Results;
using Domain.Users;
using MediatR;

namespace Application.Users.Delete.User;

public record DeleteUserCommand(UserId Id) : IRequest<Result>;