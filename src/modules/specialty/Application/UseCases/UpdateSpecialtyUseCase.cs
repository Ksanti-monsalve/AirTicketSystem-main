// src/modules/specialty/Application/UseCases/UpdateSpecialtyUseCase.cs
using AirTicketSystem.modules.specialty.Domain.Repositories;
using AirTicketSystem.modules.specialty.Domain.aggregate;
using AirTicketSystem.modules.workertype.Domain.Repositories;

namespace AirTicketSystem.modules.specialty.Application.UseCases;

public class UpdateSpecialtyUseCase
{
    private readonly ISpecialtyRepository _repository;
    private readonly IWorkerTypeRepository _workerTypeRepository;

    public UpdateSpecialtyUseCase(
        ISpecialtyRepository repository,
        IWorkerTypeRepository workerTypeRepository)
    {
        _repository          = repository;
        _workerTypeRepository = workerTypeRepository;
    }

    public async Task<Specialty> ExecuteAsync(
        int id,
        string nombre,
        int? tipoTrabajadorId,
        CancellationToken cancellationToken = default)
    {
        var especialidad = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una especialidad con ID {id}.");

        if (!string.Equals(nombre.Trim(), especialidad.Nombre.Valor, StringComparison.OrdinalIgnoreCase) &&
            await _repository.ExistsByNombreAsync(nombre))
            throw new InvalidOperationException(
                $"Ya existe otra especialidad con el nombre '{nombre.Trim()}'.");

        if (tipoTrabajadorId.HasValue)
        {
            _ = await _workerTypeRepository.FindByIdAsync(tipoTrabajadorId.Value)
                ?? throw new KeyNotFoundException(
                    $"No se encontró un tipo de trabajador con ID " +
                    $"{tipoTrabajadorId.Value}.");
        }

        especialidad.ActualizarNombre(nombre);
        especialidad.AsignarTipoTrabajador(tipoTrabajadorId);

        await _repository.UpdateAsync(especialidad);
        return especialidad;
    }
}