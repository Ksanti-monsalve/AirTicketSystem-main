// src/modules/worker/Application/UseCases/AssignWorkerSpecialtyUseCase.cs
using AirTicketSystem.modules.worker.Domain.aggregate;
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.specialty.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public sealed class AssignWorkerSpecialtyUseCase
{
    private readonly IWorkerRepository          _workerRepository;
    private readonly IWorkerSpecialtyRepository _workerSpecialtyRepository;
    private readonly ISpecialtyRepository       _specialtyRepository;

    public AssignWorkerSpecialtyUseCase(
        IWorkerRepository workerRepository,
        IWorkerSpecialtyRepository workerSpecialtyRepository,
        ISpecialtyRepository specialtyRepository)
    {
        _workerRepository          = workerRepository;
        _workerSpecialtyRepository = workerSpecialtyRepository;
        _specialtyRepository       = specialtyRepository;
    }

    public async Task<WorkerSpecialty> ExecuteAsync(
        int trabajadorId, int especialidadId, CancellationToken cancellationToken = default)
    {
        var worker = await _workerRepository.FindByIdAsync(trabajadorId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un trabajador con ID {trabajadorId}.");

        if (!worker.EstaActivo)
            throw new InvalidOperationException(
                "No se pueden asignar especialidades a un trabajador inactivo.");

        _ = await _specialtyRepository.FindByIdAsync(especialidadId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una especialidad con ID {especialidadId}.");

        if (await _workerSpecialtyRepository.ExistsByTrabajadorAndEspecialidadAsync(
            trabajadorId, especialidadId))
            throw new InvalidOperationException(
                "El trabajador ya tiene asignada esta especialidad.");

        var specialty = WorkerSpecialty.Crear(trabajadorId, especialidadId);
        await _workerSpecialtyRepository.SaveAsync(specialty);
        return specialty;
    }
}
