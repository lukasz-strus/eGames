﻿using Application.Contracts.Common;
using Application.Contracts.Games;
using Domain.Core.Results;
using MediatR;

namespace Application.Games.Create;

public record CreateDlcGameCommand(Guid FullGameId, CreateGameRequest Game) : IRequest<Result<EntityCreatedResponse>>;