// src/modules/person/Application/UseCases/GetAllPersonsUseCase.cs
using AirTicketSystem.modules.person.Domain.aggregate;
using AirTicketSystem.modules.person.Domain.Repositories;

namespace AirTicketSystem.modules.person.Application.UseCases;

public sealed class GetAllPersonsUseCase
{
    private readonly IPersonRepository _repository;

    public GetAllPersonsUseCase(IPersonRepository repository) => _repository = repository;

    public Task<IReadOnlyCollection<Person>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => _repository.FindAllAsync();
}
