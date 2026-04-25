// src/modules/aircraftmanufacturer/Application/UseCases/DeleteAircraftManufacturerUseCase.cs
using AirTicketSystem.modules.aircraftmanufacturer.Domain.Repositories;

namespace AirTicketSystem.modules.aircraftmanufacturer.Application.UseCases;

public class DeleteAircraftManufacturerUseCase
{
    private readonly IAircraftManufacturerRepository _repository;

    public DeleteAircraftManufacturerUseCase(
        IAircraftManufacturerRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        _ = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un fabricante con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}