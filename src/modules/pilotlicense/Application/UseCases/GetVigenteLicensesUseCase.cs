// src/modules/pilotlicense/Application/UseCases/GetVigenteLicensesUseCase.cs
using AirTicketSystem.modules.pilotlicense.Domain.aggregate;
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;

namespace AirTicketSystem.modules.pilotlicense.Application.UseCases;

public sealed class GetVigenteLicensesUseCase
{
    private readonly IPilotLicenseRepository _repository;

    public GetVigenteLicensesUseCase(IPilotLicenseRepository repository)
        => _repository = repository;

    public Task<IReadOnlyCollection<PilotLicense>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => _repository.FindVigentesAsync();
}
