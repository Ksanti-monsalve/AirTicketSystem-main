// src/modules/aircraftmodel/Application/UseCases/CreateAircraftModelUseCase.cs
using AirTicketSystem.modules.aircraftmodel.Domain.Repositories;
using AirTicketSystem.modules.aircraftmodel.Domain.aggregate;
using AirTicketSystem.modules.aircraftmodel.Domain.ValueObjects;
using AirTicketSystem.modules.aircraftmanufacturer.Domain.Repositories;

namespace AirTicketSystem.modules.aircraftmodel.Application.UseCases;

public class CreateAircraftModelUseCase
{
    private readonly IAircraftModelRepository _repository;
    private readonly IAircraftManufacturerRepository _manufacturerRepository;

    public CreateAircraftModelUseCase(
        IAircraftModelRepository repository,
        IAircraftManufacturerRepository manufacturerRepository)
    {
        _repository            = repository;
        _manufacturerRepository = manufacturerRepository;
    }

    public async Task<AircraftModel> ExecuteAsync(
        int fabricanteId,
        string nombre,
        string codigoModelo,
        int? autonomiKm,
        int? velocidadKmh,
        string? descripcion,
        CancellationToken cancellationToken = default)
    {
        _ = await _manufacturerRepository.FindByIdAsync(fabricanteId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un fabricante con ID {fabricanteId}.");

        var nombreVO  = NombreAircraftModel.Crear(nombre);
        var codigoVO  = CodigoModeloAircraftModel.Crear(codigoModelo);

        if (await _repository.ExistsByCodigoModeloAsync(codigoVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un modelo con el código '{codigoVO.Valor}'.");

        var model = AircraftModel.Crear(
            fabricanteId,
            nombreVO.Valor,
            codigoVO.Valor,
            autonomiKm,
            velocidadKmh,
            descripcion);

        await _repository.SaveAsync(model);
        return model;
    }
}