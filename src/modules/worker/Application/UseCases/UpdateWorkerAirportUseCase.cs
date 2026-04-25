// src/modules/worker/Application/UseCases/UpdateWorkerAirportUseCase.cs
using AirTicketSystem.modules.worker.Domain.aggregate;
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.airport.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public sealed class UpdateWorkerAirportUseCase
{
    private readonly IWorkerRepository  _repository;
    private readonly IAirportRepository _airportRepository;

    public UpdateWorkerAirportUseCase(
        IWorkerRepository repository,
        IAirportRepository airportRepository)
    {
        _repository        = repository;
        _airportRepository = airportRepository;
    }

    public async Task<Worker> ExecuteAsync(
        int id, int aeropuertoBaseId, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del trabajador no es válido.");

        var worker = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un trabajador con ID {id}.");

        if (!worker.EstaActivo)
            throw new InvalidOperationException(
                "No se puede cambiar el aeropuerto base de un trabajador inactivo.");

        var aeropuerto = await _airportRepository.FindByIdAsync(aeropuertoBaseId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un aeropuerto con ID {aeropuertoBaseId}.");

        if (!aeropuerto.Activo.Valor)
            throw new InvalidOperationException(
                $"El aeropuerto '{aeropuerto.Nombre.Valor}' está inactivo.");

        worker.ActualizarAeropuertoBase(aeropuertoBaseId);
        await _repository.UpdateAsync(worker);
        return worker;
    }
}
