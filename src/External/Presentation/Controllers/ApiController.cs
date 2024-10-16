using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api")]
public abstract class ApiController(IMediator mediator) : ControllerBase
{
    protected IMediator Mediator { get; } = mediator;
}