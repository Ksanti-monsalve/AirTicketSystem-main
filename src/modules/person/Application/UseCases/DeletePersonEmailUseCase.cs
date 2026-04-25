// src/modules/person/Application/UseCases/DeletePersonEmailUseCase.cs
using AirTicketSystem.modules.person.Domain.Repositories;

namespace AirTicketSystem.modules.person.Application.UseCases;

public sealed class DeletePersonEmailUseCase
{
    private readonly IPersonEmailRepository _repository;

    public DeletePersonEmailUseCase(IPersonEmailRepository repository)
        => _repository = repository;

    public async Task ExecuteAsync(int emailId, CancellationToken cancellationToken = default)
    {
        if (emailId <= 0)
            throw new ArgumentException("El ID del email no es válido.");

        _ = await _repository.FindByIdAsync(emailId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un email con ID {emailId}.");

        await _repository.DeleteAsync(emailId);
    }
}
