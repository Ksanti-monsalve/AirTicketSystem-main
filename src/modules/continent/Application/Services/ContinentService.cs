// src/modules/continent/Application/Services/ContinentService.cs
using AirTicketSystem.modules.continent.Application.Interfaces;
using AirTicketSystem.modules.continent.Application.UseCases;
using AirTicketSystem.modules.continent.Domain.aggregate;

namespace AirTicketSystem.modules.continent.Application.Services;

public sealed class ContinentService : IContinentService
{
    private readonly CreateContinentUseCase _create;
    private readonly GetContinentByIdUseCase _getById;
    private readonly GetAllContinentsUseCase _getAll;
    private readonly UpdateContinentUseCase _update;
    private readonly DeleteContinentUseCase _delete;

    public ContinentService(
        CreateContinentUseCase create,
        GetContinentByIdUseCase getById,
        GetAllContinentsUseCase getAll,
        UpdateContinentUseCase update,
        DeleteContinentUseCase delete)
    {
        _create  = create;
        _getById = getById;
        _getAll  = getAll;
        _update  = update;
        _delete  = delete;
    }

    public Task<Continent> CreateAsync(string nombre, string codigo)
        => _create.ExecuteAsync(nombre, codigo);

    public Task<Continent> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<Continent>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<Continent> UpdateAsync(int id, string nombre, string codigo)
        => _update.ExecuteAsync(id, nombre, codigo);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}
