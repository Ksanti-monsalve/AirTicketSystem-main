// src/modules/fare/Application/UseCases/GetFareByIdUseCase.cs
using AirTicketSystem.modules.fare.Domain.aggregate;
using AirTicketSystem.modules.fare.Domain.Repositories;

namespace AirTicketSystem.modules.fare.Application.UseCases;

public sealed class GetFareByIdUseCase
{
    private readonly IFareRepository _repository;

    public GetFareByIdUseCase(IFareRepository repository) => _repository = repository;

    public async Task<Fare> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la tarifa no es válido.");

        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una tarifa con ID {id}.");
    }
}
