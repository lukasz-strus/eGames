using Domain.Users;
using MediatR;

namespace Application.Users.Create;

public record CreateUserCommand : IRequest<UserId>;