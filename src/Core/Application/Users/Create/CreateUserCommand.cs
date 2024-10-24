using Application.Contracts.User;
using Domain.Core.Results;
using Domain.Users;
using MediatR;
using Application.Contracts.Common;

namespace Application.Users.Create;

public record CreateUserCommand(CreateUserRequest User) : IRequest<Result<EntityCreatedResponse>>;