// src/modules/pilotrating/Application/UseCases/CreatePilotRatingUseCase.cs
using AirTicketSystem.modules.pilotrating.Domain.aggregate;
using AirTicketSystem.modules.pilotrating.Domain.Repositories;
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;
using AirTicketSystem.modules.aircraftmodel.Domain.Repositories;

namespace AirTicketSystem.modules.pilotrating.Application.UseCases;

public sealed class CreatePilotRatingUseCase
{
    private readonly IPilotRatingRepository _repository;
    private readonly IPilotLicenseRepository _licenseRepository;
    private readonly IAircraftModelRepository _modelRepository;

    public CreatePilotRatingUseCase(
        IPilotRatingRepository repository,
        IPilotLicenseRepository licenseRepository,
        IAircraftModelRepository modelRepository)
    {
        _repository        = repository;
        _licenseRepository = licenseRepository;
        _modelRepository   = modelRepository;
    }

    public async Task<PilotRating> ExecuteAsync(
        int licenciaId,
        int modeloAvionId,
        DateOnly fechaHabilitacion,
        DateOnly fechaVencimiento,
        CancellationToken cancellationToken = default)
    {
        var licencia = await _licenseRepository.FindByIdAsync(licenciaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una licencia con ID {licenciaId}.");

        if (!licencia.Activa.Valor)
            throw new InvalidOperationException(
                "No se pueden crear habilitaciones sobre una licencia suspendida o inactiva.");

        if (!licencia.EstaVigente)
            throw new InvalidOperationException(
                "No se pueden crear habilitaciones sobre una licencia vencida. Renuévela primero.");

        _ = await _modelRepository.FindByIdAsync(modeloAvionId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un modelo de avión con ID {modeloAvionId}.");

        if (await _repository.ExistsByLicenciaAndModeloAsync(licenciaId, modeloAvionId))
            throw new InvalidOperationException(
                "Ya existe una habilitación para esta licencia y modelo de avión.");

        var rating = PilotRating.Crear(
            licenciaId, modeloAvionId, fechaHabilitacion, fechaVencimiento);

        await _repository.SaveAsync(rating);
        return rating;
    }
}
