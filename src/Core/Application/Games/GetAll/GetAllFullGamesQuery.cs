﻿using Application.Contracts.Games;
using Domain.Core.Results;
using MediatR;

namespace Application.Games.GetAll;

public record GetAllFullGamesQuery : IRequest<Result<FullGameListResponse>>;