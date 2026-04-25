// src/modules/pilotlicense/Application/UseCases/CreatePilotLicenseUseCase.cs
using AirTicketSystem.modules.pilotlicense.Domain.aggregate;
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;
using AirTicketSystem.modules.worker.Domain.Repositories;

namespace AirTicketSystem.modules.pilotlicense.Application.UseCases;

public sealed class CreatePilotLicenseUseCase
{
    private readonly IPilotLicenseRepository _repository;
    private readonly IWorkerRepository _workerRepository;

    public CreatePilotLicenseUseCase(
        IPilotLicenseRepository repository,
        IWorkerRepository workerRepository)
    {
        _repository       = repository;
        _workerRepository = workerRepository;
    }

    public async Task<PilotLicense> ExecuteAsync(
        int trabajadorId,
        string numeroLicencia,
        string tipoLicencia,
        DateOnly fechaExpedicion,
        DateOnly fechaVencimiento,
        string autoridadEmisora,
        CancellationToken cancellationToken = default)
    {
        var trabajador = await _workerRepository.FindByIdAsync(trabajadorId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un trabajador con ID {trabajadorId}.");

        if (!trabajador.EstaActivo)
            throw new InvalidOperationException(
                "No se pueden crear licencias para un trabajador inactivo.");

        if (await _repository.ExistsByNumeroLicenciaAsync(numeroLicencia))
            throw new InvalidOperationException(
                $"Ya existe una licencia con el número '{numeroLicencia}'.");

        var license = PilotLicense.Crear(
            trabajadorId, numeroLicencia, tipoLicencia,
            fechaExpedicion, fechaVencimiento, autoridadEmisora);

        await _repository.SaveAsync(license);
        return license;
    }
}
