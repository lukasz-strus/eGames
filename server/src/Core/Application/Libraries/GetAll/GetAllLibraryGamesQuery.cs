using Application.Contracts.Libraries;
using Domain.Core.Results;
using MediatR;
using Sieve.Models;

namespace Application.Libraries.GetAll;

public record GetAllLibraryGamesQuery(SieveModel Query, Guid? UserId = null)
    : IRequest<Result<LibraryGameListResponse>>;