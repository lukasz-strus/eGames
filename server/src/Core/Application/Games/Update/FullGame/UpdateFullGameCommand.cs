﻿using Application.Contracts.Games;
using Domain.Core.Results;
using MediatR;

namespace Application.Games.Update.FullGame;

public record UpdateFullGameCommand(Guid Id, UpdateGameRequest Game) : IRequest<Result<Unit>>;