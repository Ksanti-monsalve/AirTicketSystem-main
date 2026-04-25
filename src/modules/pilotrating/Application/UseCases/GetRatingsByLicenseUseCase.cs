// src/modules/pilotrating/Application/UseCases/GetRatingsByLicenseUseCase.cs
using AirTicketSystem.modules.pilotrating.Domain.aggregate;
using AirTicketSystem.modules.pilotrating.Domain.Repositories;

namespace AirTicketSystem.modules.pilotrating.Application.UseCases;

public sealed class GetRatingsByLicenseUseCase
{
    private readonly IPilotRatingRepository _repository;

    public GetRatingsByLicenseUseCase(IPilotRatingRepository repository)
        => _repository = repository;

    public async Task<IReadOnlyCollection<PilotRating>> ExecuteAsync(
        int licenciaId, CancellationToken cancellationToken = default)
    {
        if (licenciaId <= 0)
            throw new ArgumentException("El ID de la licencia no es válido.");

        return await _repository.FindByLicenciaAsync(licenciaId);
    }
}
