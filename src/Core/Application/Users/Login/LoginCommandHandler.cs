using Application.Abstractions;
using Domain.Users;
using MediatR;

namespace Application.Users.Login;

internal sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtProvider jwtProvider) : IRequestHandler<LoginCommand, string>
{
    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
            throw new UserNotFoundException(request.Email);

        var token = jwtProvider.Generate(user);

        return token;
    }
}