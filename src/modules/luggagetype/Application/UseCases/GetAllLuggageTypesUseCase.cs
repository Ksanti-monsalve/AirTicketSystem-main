// src/modules/luggagetype/Application/UseCases/GetAllLuggageTypesUseCase.cs
using AirTicketSystem.modules.luggagetype.Domain.aggregate;
using AirTicketSystem.modules.luggagetype.Domain.Repositories;

namespace AirTicketSystem.modules.luggagetype.Application.UseCases;

public sealed class GetAllLuggageTypesUseCase
{
    private readonly ILuggageTypeRepository _repository;

    public GetAllLuggageTypesUseCase(ILuggageTypeRepository repository)
        => _repository = repository;

    public Task<IReadOnlyCollection<LuggageType>> ExecuteAsync(CancellationToken cancellationToken = default)
        => _repository.FindAllAsync();
}
