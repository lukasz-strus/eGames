using Domain.Users;

namespace Application.Abstractions;

public interface IJwtProvider
{
    string Generate(User user);
}