// src/modules/specialty/Application/UseCases/GetGeneralSpecialtiesUseCase.cs
using AirTicketSystem.modules.specialty.Domain.Repositories;
using AirTicketSystem.modules.specialty.Domain.aggregate;

namespace AirTicketSystem.modules.specialty.Application.UseCases;

public class GetGeneralSpecialtiesUseCase
{
    private readonly ISpecialtyRepository _repository;

    public GetGeneralSpecialtiesUseCase(ISpecialtyRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Specialty>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => await _repository.FindGeneralesAsync();
}