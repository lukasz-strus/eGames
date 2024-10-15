using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Contracts;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Presentation.Controllers;

public abstract class ApiController(IMediator mediator) : ControllerBase
{
    protected IMediator Mediator { get; } = mediator;

    protected IActionResult BadRequest(Error error) => BadRequest(new ApiErrorResponse([error]));

    protected IActionResult NotFound(Error error)
    {
        return NotFound();
    }
}
