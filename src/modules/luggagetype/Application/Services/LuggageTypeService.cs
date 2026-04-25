// src/modules/luggagetype/Application/Services/LuggageTypeService.cs
using AirTicketSystem.modules.luggagetype.Application.Interfaces;
using AirTicketSystem.modules.luggagetype.Application.UseCases;
using AirTicketSystem.modules.luggagetype.Domain.aggregate;

namespace AirTicketSystem.modules.luggagetype.Application.Services;

public sealed class LuggageTypeService : ILuggageTypeService
{
    private readonly CreateLuggageTypeUseCase _create;
    private readonly GetLuggageTypeByIdUseCase _getById;
    private readonly GetAllLuggageTypesUseCase _getAll;
    private readonly UpdateLuggageTypeUseCase _update;
    private readonly DeleteLuggageTypeUseCase _delete;

    public LuggageTypeService(
        CreateLuggageTypeUseCase create,
        GetLuggageTypeByIdUseCase getById,
        GetAllLuggageTypesUseCase getAll,
        UpdateLuggageTypeUseCase update,
        DeleteLuggageTypeUseCase delete)
    {
        _create  = create;
        _getById = getById;
        _getAll  = getAll;
        _update  = update;
        _delete  = delete;
    }

    public Task<LuggageType> CreateAsync(string nombre)
        => _create.ExecuteAsync(nombre);

    public Task<LuggageType> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<LuggageType>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<LuggageType> UpdateAsync(int id, string nombre)
        => _update.ExecuteAsync(id, nombre);

    public Task DeleteAsync(int id) => _delete.ExecuteAsync(id);
}
