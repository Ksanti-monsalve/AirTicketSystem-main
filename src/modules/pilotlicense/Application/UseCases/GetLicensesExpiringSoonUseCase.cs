// src/modules/pilotlicense/Application/UseCases/GetLicensesExpiringSoonUseCase.cs
using AirTicketSystem.modules.pilotlicense.Domain.aggregate;
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;

namespace AirTicketSystem.modules.pilotlicense.Application.UseCases;

public sealed class GetLicensesExpiringSoonUseCase
{
    private readonly IPilotLicenseRepository _repository;

    public GetLicensesExpiringSoonUseCase(IPilotLicenseRepository repository)
        => _repository = repository;

    public async Task<IReadOnlyCollection<PilotLicense>> ExecuteAsync(
        int diasUmbral, CancellationToken cancellationToken = default)
    {
        if (diasUmbral <= 0)
            throw new ArgumentException("El umbral de días debe ser mayor a 0.");

        if (diasUmbral > 365)
            throw new ArgumentException("El umbral no puede superar 365 días.");

        return await _repository.FindProximasAVencerAsync(diasUmbral);
    }
}
