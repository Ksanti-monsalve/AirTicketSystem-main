// src/modules/person/Application/UseCases/DeletePersonUseCase.cs
using AirTicketSystem.modules.person.Domain.Repositories;

namespace AirTicketSystem.modules.person.Application.UseCases;

public sealed class DeletePersonUseCase
{
    private readonly IPersonRepository _repository;

    public DeletePersonUseCase(IPersonRepository repository) => _repository = repository;

    public async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la persona no es válido.");

        _ = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una persona con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}
