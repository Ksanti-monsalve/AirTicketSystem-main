// src/modules/aircraftmanufacturer/Application/UseCases/CreateAircraftManufacturerUseCase.cs
using AirTicketSystem.modules.aircraftmanufacturer.Domain.Repositories;
using AirTicketSystem.modules.aircraftmanufacturer.Domain.aggregate;
using AirTicketSystem.modules.aircraftmanufacturer.Domain.ValueObjects;
using AirTicketSystem.modules.country.Domain.Repositories;

namespace AirTicketSystem.modules.aircraftmanufacturer.Application.UseCases;

public class CreateAircraftManufacturerUseCase
{
    private readonly IAircraftManufacturerRepository _repository;
    private readonly ICountryRepository _countryRepository;

    public CreateAircraftManufacturerUseCase(
        IAircraftManufacturerRepository repository,
        ICountryRepository countryRepository)
    {
        _repository        = repository;
        _countryRepository = countryRepository;
    }

    public async Task<AircraftManufacturer> ExecuteAsync(
        int paisId,
        string nombre,
        string? sitioWeb,
        CancellationToken cancellationToken = default)
    {
        _ = await _countryRepository.FindByIdAsync(paisId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un país con ID {paisId}.");

        var nombreVO = NombreAircraftManufacturer.Crear(nombre);

        if (await _repository.ExistsByNombreAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un fabricante con el nombre '{nombreVO.Valor}'.");

        var manufacturer = AircraftManufacturer.Crear(paisId, nombreVO.Valor, sitioWeb);
        await _repository.SaveAsync(manufacturer);
        return manufacturer;
    }
}