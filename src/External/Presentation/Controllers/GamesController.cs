using Application.Contracts.Common;
using Application.Contracts.Games;
using Application.Games.Create;
using Application.Games.Get;
using Application.Games.GetAll;
using Domain;
using Domain.Core.Results;
using Domain.Core.Results.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Contracts;

namespace Presentation.Controllers;

public class GamesController(IMediator mediator) : ApiController(mediator)
{
    [HttpGet(ApiRoutes.Games.GetGames)]
    [ProducesResponseType(typeof(GameListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetGames(CancellationToken cancellationToken) =>
        await Result.Success(new GetAllGamesQuery())
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match<GameListResponse, IActionResult>(Ok, BadRequest);

    [HttpGet(ApiRoutes.Games.GetGame)]
    [ProducesResponseType(typeof(GameResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGameById(Guid id, CancellationToken cancellationToken) =>
        await Result.Success(new GetGameByIdQuery(id))
            .Bind(query => Mediator.Send(query, cancellationToken))
            .Match<GameResponse, IActionResult>(Ok, NotFound);

    [Authorize("Admin")]
    [HttpPost(ApiRoutes.Games.CreateGame)]
    [ProducesResponseType(typeof(EntityCreatedResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateGame(
        [FromBody] CreateGameRequest request,
        CancellationToken cancellationToken) =>
        await Result.Create(request, Errors.General.BadRequest)
            .Map(value => new CreateFullGameCommand(value))
            .Bind(command => Mediator.Send(command, cancellationToken))
            .Match<EntityCreatedResponse, IActionResult>(
                entityCreated => CreatedAtAction(nameof(GetGameById), new { id = entityCreated.Id }, entityCreated),
                BadRequest);

    //TODO: Podzielić na fullgame itp może inne tabele?
    //TODO: Refactor excpetion middleware
    //TODO: Add logging
}