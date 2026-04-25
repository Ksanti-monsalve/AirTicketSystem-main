// src/modules/luggage/Application/UseCases/ReportDamagedLuggageUseCase.cs
using AirTicketSystem.modules.luggage.Domain.aggregate;
using AirTicketSystem.modules.luggage.Domain.Repositories;

namespace AirTicketSystem.modules.luggage.Application.UseCases;

public sealed class ReportDamagedLuggageUseCase
{
    private readonly ILuggageRepository _repository;

    public ReportDamagedLuggageUseCase(ILuggageRepository repository) => _repository = repository;

    public async Task<Luggage> ExecuteAsync(
        int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del equipaje no es válido.");

        var luggage = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"No se encontró el equipaje con ID {id}.");

        luggage.ReportarDanado();
        await _repository.UpdateAsync(luggage);
        return luggage;
    }
}
