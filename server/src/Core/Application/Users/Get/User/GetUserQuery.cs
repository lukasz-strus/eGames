using Application.Contracts.User;
using Domain.Core.Results;
using MediatR;

namespace Application.Users.Get.User;

public record GetUserQuery(Guid UserId) : IRequest<Result<UserResponse>>;