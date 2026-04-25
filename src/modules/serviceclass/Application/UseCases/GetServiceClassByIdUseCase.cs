// src/modules/serviceclass/Application/UseCases/GetServiceClassByIdUseCase.cs
using AirTicketSystem.modules.serviceclass.Domain.Repositories;
using AirTicketSystem.modules.serviceclass.Domain.aggregate;

namespace AirTicketSystem.modules.serviceclass.Application.UseCases;

public class GetServiceClassByIdUseCase
{
    private readonly IServiceClassRepository _repository;

    public GetServiceClassByIdUseCase(IServiceClassRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceClass> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException(
                "El ID de la clase de servicio no es válido.");

        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una clase de servicio con ID {id}.");
    }
}