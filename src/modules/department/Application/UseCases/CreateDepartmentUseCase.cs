// src/modules/department/Application/UseCases/CreateDepartmentUseCase.cs
using AirTicketSystem.modules.department.Domain.aggregate;
using AirTicketSystem.modules.department.Domain.Repositories;
using AirTicketSystem.modules.department.Domain.ValueObjects;
using AirTicketSystem.modules.region.Domain.Repositories;

namespace AirTicketSystem.modules.department.Application.UseCases;

public sealed class CreateDepartmentUseCase
{
    private readonly IDepartmentRepository _repository;
    private readonly IRegionRepository _regionRepository;

    public CreateDepartmentUseCase(
        IDepartmentRepository repository,
        IRegionRepository regionRepository)
    {
        _repository       = repository;
        _regionRepository = regionRepository;
    }

    public async Task<Department> ExecuteAsync(
        int regionId,
        string nombre,
        string? codigo,
        CancellationToken cancellationToken = default)
    {
        _ = await _regionRepository.FindByIdAsync(regionId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una región con ID {regionId}.");

        var nombreVO = NombreDepartment.Crear(nombre);

        if (await _repository.ExistsByNombreAndRegionAsync(nombreVO.Valor, regionId))
            throw new InvalidOperationException(
                $"Ya existe un departamento con el nombre '{nombreVO.Valor}' " +
                $"en la región con ID {regionId}.");

        string? codigoNormalizado = codigo is not null
            ? CodigoDepartment.Crear(codigo).Valor
            : null;

        var department = Department.Crear(regionId, nombreVO.Valor, codigoNormalizado);
        await _repository.SaveAsync(department);
        return department;
    }
}
