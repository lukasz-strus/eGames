using Domain.Users;
using MediatR;

namespace Application.Users.Delete;

public record DeleteUserCommand(UserId Id) : IRequest<Unit>;