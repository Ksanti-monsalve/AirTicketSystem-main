// src/modules/aircraftmanufacturer/Application/UseCases/UpdateAircraftManufacturerUseCase.cs
using AirTicketSystem.modules.aircraftmanufacturer.Domain.Repositories;
using AirTicketSystem.modules.aircraftmanufacturer.Domain.aggregate;
using AirTicketSystem.modules.aircraftmanufacturer.Domain.ValueObjects;

namespace AirTicketSystem.modules.aircraftmanufacturer.Application.UseCases;

public class UpdateAircraftManufacturerUseCase
{
    private readonly IAircraftManufacturerRepository _repository;

    public UpdateAircraftManufacturerUseCase(
        IAircraftManufacturerRepository repository)
    {
        _repository = repository;
    }

    public async Task<AircraftManufacturer> ExecuteAsync(
        int id,
        string nombre,
        string? sitioWeb,
        CancellationToken cancellationToken = default)
    {
        var fabricante = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un fabricante con ID {id}.");

        var nombreVO = NombreAircraftManufacturer.Crear(nombre);

        if (nombreVO.Valor != fabricante.Nombre.Valor &&
            await _repository.ExistsByNombreAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otro fabricante con el nombre '{nombreVO.Valor}'.");

        fabricante.ActualizarNombre(nombreVO.Valor);
        fabricante.ActualizarSitioWeb(sitioWeb);

        await _repository.UpdateAsync(fabricante);
        return fabricante;
    }
}