﻿using Application.Contracts.Games;
using Application.Games.GetAll;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Contracts;

namespace Presentation.Controllers;

public class GameController(IMediator mediator) : ApiController(mediator)
{
    [HttpGet(ApiRoutes.Games.GetGames)]
    [ProducesResponseType(typeof(GameListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGames(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetAllGamesQuery(), cancellationToken);

        return Ok(result);
    }
}