using Application.Contracts.User;
using Domain.Core.Results;
using MediatR;
using Application.Contracts.Common;

namespace Application.Users.Create.User;

public record CreateUserCommand(CreateUserRequest User) : IRequest<Result<EntityCreatedResponse>>;