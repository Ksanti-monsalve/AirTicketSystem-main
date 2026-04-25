// src/modules/fare/Application/UseCases/GetAllFaresUseCase.cs
using AirTicketSystem.modules.fare.Domain.aggregate;
using AirTicketSystem.modules.fare.Domain.Repositories;

namespace AirTicketSystem.modules.fare.Application.UseCases;

public sealed class GetAllFaresUseCase
{
    private readonly IFareRepository _repository;

    public GetAllFaresUseCase(IFareRepository repository) => _repository = repository;

    public Task<IReadOnlyCollection<Fare>> ExecuteAsync(CancellationToken cancellationToken = default)
        => _repository.FindAllAsync();
}
