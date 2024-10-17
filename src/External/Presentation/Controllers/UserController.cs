using Application.Users.Login;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Contracts;

namespace Presentation.Controllers;

public sealed class UserController(IMediator mediator) : ApiController(mediator)
{
    [HttpPost(ApiRoutes.Users.Login)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new LoginCommand(request.Email), cancellationToken);

        return Ok(result);
    }
}