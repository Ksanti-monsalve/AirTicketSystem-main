// src/modules/person/Application/UseCases/GetPersonByIdUseCase.cs
using AirTicketSystem.modules.person.Domain.aggregate;
using AirTicketSystem.modules.person.Domain.Repositories;

namespace AirTicketSystem.modules.person.Application.UseCases;

public sealed class GetPersonByIdUseCase
{
    private readonly IPersonRepository _repository;

    public GetPersonByIdUseCase(IPersonRepository repository) => _repository = repository;

    public async Task<Person> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la persona no es válido.");

        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una persona con ID {id}.");
    }
}
