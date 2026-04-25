// src/modules/aircraftmanufacturer/Application/Services/AircraftManufacturerService.cs
using AirTicketSystem.modules.aircraftmanufacturer.Application.Interfaces;
using AirTicketSystem.modules.aircraftmanufacturer.Application.UseCases;
using AirTicketSystem.modules.aircraftmanufacturer.Domain.aggregate;

namespace AirTicketSystem.modules.aircraftmanufacturer.Application.Services;

public class AircraftManufacturerService : IAircraftManufacturerService
{
    private readonly CreateAircraftManufacturerUseCase _create;
    private readonly GetAircraftManufacturerByIdUseCase _getById;
    private readonly GetAllAircraftManufacturersUseCase _getAll;
    private readonly GetAircraftManufacturersByCountryUseCase _getByCountry;
    private readonly UpdateAircraftManufacturerUseCase _update;
    private readonly DeleteAircraftManufacturerUseCase _delete;

    public AircraftManufacturerService(
        CreateAircraftManufacturerUseCase create,
        GetAircraftManufacturerByIdUseCase getById,
        GetAllAircraftManufacturersUseCase getAll,
        GetAircraftManufacturersByCountryUseCase getByCountry,
        UpdateAircraftManufacturerUseCase update,
        DeleteAircraftManufacturerUseCase delete)
    {
        _create      = create;
        _getById     = getById;
        _getAll      = getAll;
        _getByCountry = getByCountry;
        _update      = update;
        _delete      = delete;
    }

    public Task<AircraftManufacturer> CreateAsync(
        int paisId, string nombre, string? sitioWeb)
        => _create.ExecuteAsync(paisId, nombre, sitioWeb);

    public Task<AircraftManufacturer> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<AircraftManufacturer>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<IReadOnlyCollection<AircraftManufacturer>> GetByCountryAsync(int paisId)
        => _getByCountry.ExecuteAsync(paisId);

    public Task<AircraftManufacturer> UpdateAsync(
        int id, string nombre, string? sitioWeb)
        => _update.ExecuteAsync(id, nombre, sitioWeb);

    public Task DeleteAsync(int id) => _delete.ExecuteAsync(id);
}