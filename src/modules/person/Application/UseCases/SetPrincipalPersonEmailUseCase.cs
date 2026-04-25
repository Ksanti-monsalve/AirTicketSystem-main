// src/modules/person/Application/UseCases/SetPrincipalPersonEmailUseCase.cs
using AirTicketSystem.modules.person.Domain.aggregate;
using AirTicketSystem.modules.person.Domain.Repositories;

namespace AirTicketSystem.modules.person.Application.UseCases;

public sealed class SetPrincipalPersonEmailUseCase
{
    private readonly IPersonEmailRepository _repository;

    public SetPrincipalPersonEmailUseCase(IPersonEmailRepository repository)
        => _repository = repository;

    public async Task<PersonEmail> ExecuteAsync(
        int emailId, CancellationToken cancellationToken = default)
    {
        if (emailId <= 0)
            throw new ArgumentException("El ID del email no es válido.");

        var email = await _repository.FindByIdAsync(emailId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un email con ID {emailId}.");

        await _repository.DesmarcarPrincipalByPersonaAsync(email.PersonaId);

        email.MarcarComoPrincipal();
        await _repository.UpdateAsync(email);
        return email;
    }
}
