// src/modules/specialty/Application/Services/SpecialtyService.cs
using AirTicketSystem.modules.specialty.Application.Interfaces;
using AirTicketSystem.modules.specialty.Application.UseCases;
using AirTicketSystem.modules.specialty.Domain.aggregate;

namespace AirTicketSystem.modules.specialty.Application.Services;

public class SpecialtyService : ISpecialtyService
{
    private readonly CreateSpecialtyUseCase _create;
    private readonly GetSpecialtyByIdUseCase _getById;
    private readonly GetAllSpecialtiesUseCase _getAll;
    private readonly GetSpecialtiesByWorkerTypeUseCase _getByWorkerType;
    private readonly GetGeneralSpecialtiesUseCase _getGenerales;
    private readonly UpdateSpecialtyUseCase _update;
    private readonly DeleteSpecialtyUseCase _delete;

    public SpecialtyService(
        CreateSpecialtyUseCase create,
        GetSpecialtyByIdUseCase getById,
        GetAllSpecialtiesUseCase getAll,
        GetSpecialtiesByWorkerTypeUseCase getByWorkerType,
        GetGeneralSpecialtiesUseCase getGenerales,
        UpdateSpecialtyUseCase update,
        DeleteSpecialtyUseCase delete)
    {
        _create         = create;
        _getById        = getById;
        _getAll         = getAll;
        _getByWorkerType = getByWorkerType;
        _getGenerales   = getGenerales;
        _update         = update;
        _delete         = delete;
    }

    public Task<Specialty> CreateAsync(string nombre, int? tipoTrabajadorId)
        => _create.ExecuteAsync(nombre, tipoTrabajadorId);

    public Task<Specialty> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<Specialty>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<IReadOnlyCollection<Specialty>> GetByWorkerTypeAsync(int tipoTrabajadorId)
        => _getByWorkerType.ExecuteAsync(tipoTrabajadorId);

    public Task<IReadOnlyCollection<Specialty>> GetGeneralesAsync()
        => _getGenerales.ExecuteAsync();

    public Task<Specialty> UpdateAsync(
        int id, string nombre, int? tipoTrabajadorId)
        => _update.ExecuteAsync(id, nombre, tipoTrabajadorId);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}