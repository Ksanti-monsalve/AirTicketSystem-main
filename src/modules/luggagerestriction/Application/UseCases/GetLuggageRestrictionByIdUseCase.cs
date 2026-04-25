// src/modules/luggagerestriction/Application/UseCases/GetLuggageRestrictionByIdUseCase.cs
using AirTicketSystem.modules.luggagerestriction.Domain.aggregate;
using AirTicketSystem.modules.luggagerestriction.Domain.Repositories;

namespace AirTicketSystem.modules.luggagerestriction.Application.UseCases;

public sealed class GetLuggageRestrictionByIdUseCase
{
    private readonly ILuggageRestrictionRepository _repository;

    public GetLuggageRestrictionByIdUseCase(ILuggageRestrictionRepository repository)
        => _repository = repository;

    public async Task<LuggageRestriction> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la restricción no es válido.");

        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una restricción de equipaje con ID {id}.");
    }
}
