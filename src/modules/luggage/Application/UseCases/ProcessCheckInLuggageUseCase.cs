// src/modules/luggage/Application/UseCases/ProcessCheckInLuggageUseCase.cs
using AirTicketSystem.modules.luggage.Domain.aggregate;
using AirTicketSystem.modules.luggage.Domain.Repositories;

namespace AirTicketSystem.modules.luggage.Application.UseCases;

public sealed class ProcessCheckInLuggageUseCase
{
    private readonly ILuggageRepository _repository;

    public ProcessCheckInLuggageUseCase(ILuggageRepository repository) => _repository = repository;

    public async Task<Luggage> ExecuteAsync(
        int id,
        decimal pesoRealKg,
        decimal pesoMaximoPermitido,
        decimal costoPorKgExcedido,
        int? largoRealCm = null,
        int? anchoRealCm = null,
        int? altoRealCm = null,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del equipaje no es válido.");

        var luggage = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"No se encontró el equipaje con ID {id}.");

        luggage.RegistrarEnCheckin(
            pesoRealKg,
            pesoMaximoPermitido,
            costoPorKgExcedido,
            largoRealCm,
            anchoRealCm,
            altoRealCm);

        await _repository.UpdateAsync(luggage);
        return luggage;
    }
}
