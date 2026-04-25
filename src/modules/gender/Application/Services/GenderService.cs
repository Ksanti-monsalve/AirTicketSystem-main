// src/modules/gender/Application/Services/GenderService.cs
using AirTicketSystem.modules.gender.Application.Interfaces;
using AirTicketSystem.modules.gender.Application.UseCases;
using AirTicketSystem.modules.gender.Domain.aggregate;

namespace AirTicketSystem.modules.gender.Application.Services;

public sealed class GenderService : IGenderService
{
    private readonly CreateGenderUseCase _create;
    private readonly GetGenderByIdUseCase _getById;
    private readonly GetAllGendersUseCase _getAll;
    private readonly UpdateGenderUseCase _update;
    private readonly DeleteGenderUseCase _delete;

    public GenderService(
        CreateGenderUseCase create,
        GetGenderByIdUseCase getById,
        GetAllGendersUseCase getAll,
        UpdateGenderUseCase update,
        DeleteGenderUseCase delete)
    {
        _create  = create;
        _getById = getById;
        _getAll  = getAll;
        _update  = update;
        _delete  = delete;
    }

    public Task<Gender> CreateAsync(string nombre)
        => _create.ExecuteAsync(nombre);

    public Task<Gender> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<Gender>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<Gender> UpdateAsync(int id, string nombre)
        => _update.ExecuteAsync(id, nombre);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}
