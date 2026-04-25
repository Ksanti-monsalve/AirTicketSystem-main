// src/modules/specialty/Application/UseCases/GetSpecialtiesByWorkerTypeUseCase.cs
using AirTicketSystem.modules.specialty.Domain.Repositories;
using AirTicketSystem.modules.specialty.Domain.aggregate;

namespace AirTicketSystem.modules.specialty.Application.UseCases;

public class GetSpecialtiesByWorkerTypeUseCase
{
    private readonly ISpecialtyRepository _repository;

    public GetSpecialtiesByWorkerTypeUseCase(ISpecialtyRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Specialty>> ExecuteAsync(
        int tipoTrabajadorId,
        CancellationToken cancellationToken = default)
    {
        if (tipoTrabajadorId <= 0)
            throw new ArgumentException(
                "El ID del tipo de trabajador no es válido.");

        return await _repository.FindByTipoTrabajadorAsync(tipoTrabajadorId);
    }
}