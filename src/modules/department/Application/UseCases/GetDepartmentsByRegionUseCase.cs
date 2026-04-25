// src/modules/department/Application/UseCases/GetDepartmentsByRegionUseCase.cs
using AirTicketSystem.modules.department.Domain.aggregate;
using AirTicketSystem.modules.department.Domain.Repositories;

namespace AirTicketSystem.modules.department.Application.UseCases;

public sealed class GetDepartmentsByRegionUseCase
{
    private readonly IDepartmentRepository _repository;

    public GetDepartmentsByRegionUseCase(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Department>> ExecuteAsync(
        int regionId,
        CancellationToken cancellationToken = default)
    {
        if (regionId <= 0)
            throw new ArgumentException("El ID de la región no es válido.");

        return await _repository.FindByRegionAsync(regionId);
    }
}
