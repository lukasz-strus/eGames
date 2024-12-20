﻿using Application.Contracts.Games;
using Domain.Core.Results;
using MediatR;

namespace Application.Games.Get.FullGame;

public record GetFullGameByIdQuery(Guid Id) : IRequest<Result<FullGameResponse>>;