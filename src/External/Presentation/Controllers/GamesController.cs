using Application.Contracts.Games;
using Application.Games.Get;
using Application.Games.GetAll;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Contracts;

namespace Presentation.Controllers;

public class GamesController(IMediator mediator) : ApiController(mediator)
{
    [HttpGet(ApiRoutes.Games.GetGames)]
    [ProducesResponseType(typeof(GameListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetGames(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetAllGamesQuery(), cancellationToken);

        return Ok(result);
    }

    [HttpGet(ApiRoutes.Games.GetGame)]
    [ProducesResponseType(typeof(GameResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGameById(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetGameByIdQuery(id), cancellationToken);

        return Ok(result);
    }
}