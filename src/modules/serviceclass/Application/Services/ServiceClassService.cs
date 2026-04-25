// src/modules/serviceclass/Application/Services/ServiceClassService.cs
using AirTicketSystem.modules.serviceclass.Application.Interfaces;
using AirTicketSystem.modules.serviceclass.Application.UseCases;
using AirTicketSystem.modules.serviceclass.Domain.aggregate;

namespace AirTicketSystem.modules.serviceclass.Application.Services;

public class ServiceClassService : IServiceClassService
{
    private readonly CreateServiceClassUseCase _create;
    private readonly GetServiceClassByIdUseCase _getById;
    private readonly GetAllServiceClassesUseCase _getAll;
    private readonly UpdateServiceClassUseCase _update;
    private readonly DeleteServiceClassUseCase _delete;

    public ServiceClassService(
        CreateServiceClassUseCase create,
        GetServiceClassByIdUseCase getById,
        GetAllServiceClassesUseCase getAll,
        UpdateServiceClassUseCase update,
        DeleteServiceClassUseCase delete)
    {
        _create  = create;
        _getById = getById;
        _getAll  = getAll;
        _update  = update;
        _delete  = delete;
    }

    public Task<ServiceClass> CreateAsync(
        string nombre, string codigo, string? descripcion)
        => _create.ExecuteAsync(nombre, codigo, descripcion);

    public Task<ServiceClass> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<ServiceClass>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<ServiceClass> UpdateAsync(
        int id, string nombre, string? descripcion)
        => _update.ExecuteAsync(id, nombre, descripcion);

    public Task DeleteAsync(int id) => _delete.ExecuteAsync(id);
}