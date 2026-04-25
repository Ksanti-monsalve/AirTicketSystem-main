// src/modules/serviceclass/Application/UseCases/UpdateServiceClassUseCase.cs
using AirTicketSystem.modules.serviceclass.Domain.Repositories;
using AirTicketSystem.modules.serviceclass.Domain.aggregate;

namespace AirTicketSystem.modules.serviceclass.Application.UseCases;

public class UpdateServiceClassUseCase
{
    private readonly IServiceClassRepository _repository;

    public UpdateServiceClassUseCase(IServiceClassRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceClass> ExecuteAsync(
        int id,
        string nombre,
        string? descripcion,
        CancellationToken cancellationToken = default)
    {
        var clase = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una clase de servicio con ID {id}.");

        clase.ActualizarNombre(nombre);
        clase.ActualizarDescripcion(descripcion);

        await _repository.UpdateAsync(clase);
        return clase;
    }
}