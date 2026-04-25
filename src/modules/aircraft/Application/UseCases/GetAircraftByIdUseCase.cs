// src/modules/aircraft/Application/UseCases/GetAircraftByIdUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Domain.aggregate;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class GetAircraftByIdUseCase
{
    private readonly IAircraftRepository _repository;

    public GetAircraftByIdUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public async Task<Aircraft> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del avión no es válido.");

        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un avión con ID {id}.");
    }
}