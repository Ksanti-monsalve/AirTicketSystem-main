// src/modules/luggagerestriction/Application/UseCases/GetAllRestrictionsUseCase.cs
using AirTicketSystem.modules.luggagerestriction.Domain.aggregate;
using AirTicketSystem.modules.luggagerestriction.Domain.Repositories;

namespace AirTicketSystem.modules.luggagerestriction.Application.UseCases;

public sealed class GetAllRestrictionsUseCase
{
    private readonly ILuggageRestrictionRepository _repository;

    public GetAllRestrictionsUseCase(ILuggageRestrictionRepository repository)
        => _repository = repository;

    public Task<IReadOnlyCollection<LuggageRestriction>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => _repository.FindAllAsync();
}
