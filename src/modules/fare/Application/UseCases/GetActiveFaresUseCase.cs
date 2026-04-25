// src/modules/fare/Application/UseCases/GetActiveFaresUseCase.cs
using AirTicketSystem.modules.fare.Domain.aggregate;
using AirTicketSystem.modules.fare.Domain.Repositories;

namespace AirTicketSystem.modules.fare.Application.UseCases;

public sealed class GetActiveFaresUseCase
{
    private readonly IFareRepository _repository;

    public GetActiveFaresUseCase(IFareRepository repository) => _repository = repository;

    public Task<IReadOnlyCollection<Fare>> ExecuteAsync(CancellationToken cancellationToken = default)
        => _repository.FindActivasAsync();
}
