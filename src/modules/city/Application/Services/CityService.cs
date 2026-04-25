// src/modules/city/Application/Services/CityService.cs
using AirTicketSystem.modules.city.Application.Interfaces;
using AirTicketSystem.modules.city.Application.UseCases;
using AirTicketSystem.modules.city.Domain.aggregate;

namespace AirTicketSystem.modules.city.Application.Services;

public sealed class CityService : ICityService
{
    private readonly CreateCityUseCase _create;
    private readonly GetCityByIdUseCase _getById;
    private readonly GetAllCitiesUseCase _getAll;
    private readonly GetCitiesByDepartmentUseCase _getByDepartment;
    private readonly UpdateCityUseCase _update;
    private readonly DeleteCityUseCase _delete;

    public CityService(
        CreateCityUseCase create,
        GetCityByIdUseCase getById,
        GetAllCitiesUseCase getAll,
        GetCitiesByDepartmentUseCase getByDepartment,
        UpdateCityUseCase update,
        DeleteCityUseCase delete)
    {
        _create          = create;
        _getById         = getById;
        _getAll          = getAll;
        _getByDepartment = getByDepartment;
        _update          = update;
        _delete          = delete;
    }

    public Task<City> CreateAsync(int departamentoId, string nombre, string? codigoPostal)
        => _create.ExecuteAsync(departamentoId, nombre, codigoPostal);

    public Task<City> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<City>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<IReadOnlyCollection<City>> GetByDepartmentAsync(int departamentoId)
        => _getByDepartment.ExecuteAsync(departamentoId);

    public Task<City> UpdateAsync(int id, string nombre, string? codigoPostal)
        => _update.ExecuteAsync(id, nombre, codigoPostal);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}
