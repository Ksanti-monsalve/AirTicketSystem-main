// src/modules/fare/Application/UseCases/UpdateFareUseCase.cs
using AirTicketSystem.modules.fare.Domain.aggregate;
using AirTicketSystem.modules.fare.Domain.Repositories;

namespace AirTicketSystem.modules.fare.Application.UseCases;

public sealed class UpdateFareUseCase
{
    private readonly IFareRepository _repository;

    public UpdateFareUseCase(IFareRepository repository) => _repository = repository;

    public async Task<Fare> ExecuteAsync(
        int id,
        string nombre,
        decimal precioBase,
        decimal impuestos,
        bool permiteCambios,
        bool permiteReembolso,
        DateOnly? vigenteHasta,
        CancellationToken cancellationToken = default)
    {
        var fare = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una tarifa con ID {id}.");

        fare.ActualizarNombre(nombre);
        fare.ActualizarPrecios(precioBase, impuestos);
        fare.ActualizarPoliticas(permiteCambios, permiteReembolso);
        fare.ActualizarVigencia(vigenteHasta);
        await _repository.UpdateAsync(fare);
        return fare;
    }
}
