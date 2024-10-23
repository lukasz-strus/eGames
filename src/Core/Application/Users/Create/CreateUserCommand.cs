using Application.Contracts.User;
using Domain.Users;
using MediatR;

namespace Application.Users.Create;

public record CreateUserCommand(CreateUserRequest User) : IRequest<UserId>;