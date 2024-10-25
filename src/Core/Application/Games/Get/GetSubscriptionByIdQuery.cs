﻿using Application.Contracts.Games;
using Domain.Core.Results;
using MediatR;

namespace Application.Games.Get;

public record GetSubscriptionByIdQuery(Guid Id) : IRequest<Result<SubscriptionResponse>>;