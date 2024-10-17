using MediatR;

namespace Application.Users.Login;

public record LoginCommand(string Email) : IRequest<string>;