// src/modules/person/Application/UseCases/AddPersonEmailUseCase.cs
using AirTicketSystem.modules.person.Domain.aggregate;
using AirTicketSystem.modules.person.Domain.Repositories;

namespace AirTicketSystem.modules.person.Application.UseCases;

public sealed class AddPersonEmailUseCase
{
    private readonly IPersonRepository _personRepository;
    private readonly IPersonEmailRepository _emailRepository;

    public AddPersonEmailUseCase(
        IPersonRepository personRepository,
        IPersonEmailRepository emailRepository)
    {
        _personRepository = personRepository;
        _emailRepository  = emailRepository;
    }

    public async Task<PersonEmail> ExecuteAsync(
        int personaId,
        int tipoEmailId,
        string email,
        bool esPrincipal = false,
        CancellationToken cancellationToken = default)
    {
        _ = await _personRepository.FindByIdAsync(personaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una persona con ID {personaId}.");

        if (await _emailRepository.ExistsByEmailAsync(email))
            throw new InvalidOperationException(
                $"El email '{email}' ya está registrado.");

        if (esPrincipal)
            await _emailRepository.DesmarcarPrincipalByPersonaAsync(personaId);

        var personEmail = PersonEmail.Crear(personaId, tipoEmailId, email, esPrincipal);

        await _emailRepository.SaveAsync(personEmail);
        return personEmail;
    }
}
