// src/modules/person/Application/UseCases/AddPersonPhoneUseCase.cs
using AirTicketSystem.modules.person.Domain.aggregate;
using AirTicketSystem.modules.person.Domain.Repositories;

namespace AirTicketSystem.modules.person.Application.UseCases;

public sealed class AddPersonPhoneUseCase
{
    private readonly IPersonRepository _personRepository;
    private readonly IPersonPhoneRepository _phoneRepository;

    public AddPersonPhoneUseCase(
        IPersonRepository personRepository,
        IPersonPhoneRepository phoneRepository)
    {
        _personRepository = personRepository;
        _phoneRepository  = phoneRepository;
    }

    public async Task<PersonPhone> ExecuteAsync(
        int personaId,
        int tipoTelefonoId,
        string numero,
        string? indicativoPais = null,
        bool esPrincipal = false,
        CancellationToken cancellationToken = default)
    {
        _ = await _personRepository.FindByIdAsync(personaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una persona con ID {personaId}.");

        if (await _phoneRepository.ExistsByNumeroAndPersonaAsync(numero, personaId))
            throw new InvalidOperationException(
                $"La persona ya tiene registrado el teléfono '{numero}'.");

        if (esPrincipal)
            await _phoneRepository.DesmarcarPrincipalByPersonaAsync(personaId);

        var phone = PersonPhone.Crear(personaId, tipoTelefonoId, numero,
            indicativoPais, esPrincipal);

        await _phoneRepository.SaveAsync(phone);
        return phone;
    }
}
