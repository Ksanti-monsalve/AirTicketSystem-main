// src/modules/department/Application/Services/DepartmentService.cs
using AirTicketSystem.modules.department.Application.Interfaces;
using AirTicketSystem.modules.department.Application.UseCases;
using AirTicketSystem.modules.department.Domain.aggregate;

namespace AirTicketSystem.modules.department.Application.Services;

public sealed class DepartmentService : IDepartmentService
{
    private readonly CreateDepartmentUseCase _create;
    private readonly GetDepartmentByIdUseCase _getById;
    private readonly GetAllDepartmentsUseCase _getAll;
    private readonly GetDepartmentsByRegionUseCase _getByRegion;
    private readonly UpdateDepartmentUseCase _update;
    private readonly DeleteDepartmentUseCase _delete;

    public DepartmentService(
        CreateDepartmentUseCase create,
        GetDepartmentByIdUseCase getById,
        GetAllDepartmentsUseCase getAll,
        GetDepartmentsByRegionUseCase getByRegion,
        UpdateDepartmentUseCase update,
        DeleteDepartmentUseCase delete)
    {
        _create      = create;
        _getById     = getById;
        _getAll      = getAll;
        _getByRegion = getByRegion;
        _update      = update;
        _delete      = delete;
    }

    public Task<Department> CreateAsync(int regionId, string nombre, string? codigo)
        => _create.ExecuteAsync(regionId, nombre, codigo);

    public Task<Department> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<Department>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<IReadOnlyCollection<Department>> GetByRegionAsync(int regionId)
        => _getByRegion.ExecuteAsync(regionId);

    public Task<Department> UpdateAsync(int id, string nombre, string? codigo)
        => _update.ExecuteAsync(id, nombre, codigo);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}
