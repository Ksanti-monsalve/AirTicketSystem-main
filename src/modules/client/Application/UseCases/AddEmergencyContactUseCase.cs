// src/modules/client/Application/UseCases/AddEmergencyContactUseCase.cs
using AirTicketSystem.modules.client.Domain.aggregate;
using AirTicketSystem.modules.client.Domain.Repositories;
using AirTicketSystem.modules.person.Domain.Repositories;
using AirTicketSystem.modules.contactrelationship.Domain.Repositories;

namespace AirTicketSystem.modules.client.Application.UseCases;

public sealed class AddEmergencyContactUseCase
{
    private readonly IClientRepository               _clientRepository;
    private readonly IEmergencyContactRepository     _contactRepository;
    private readonly IPersonRepository               _personRepository;
    private readonly IContactRelationshipRepository  _relationRepository;

    public AddEmergencyContactUseCase(
        IClientRepository clientRepository,
        IEmergencyContactRepository contactRepository,
        IPersonRepository personRepository,
        IContactRelationshipRepository relationRepository)
    {
        _clientRepository   = clientRepository;
        _contactRepository  = contactRepository;
        _personRepository   = personRepository;
        _relationRepository = relationRepository;
    }

    public async Task<EmergencyContact> ExecuteAsync(
        int clienteId,
        int personaId,
        int relacionId,
        bool esPrincipal = false,
        CancellationToken cancellationToken = default)
    {
        _ = await _clientRepository.FindByIdAsync(clienteId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un cliente con ID {clienteId}.");

        _ = await _personRepository.FindByIdAsync(personaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una persona con ID {personaId}.");

        _ = await _relationRepository.FindByIdAsync(relacionId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una relación de contacto con ID {relacionId}.");

        if (await _contactRepository.ExistsByClienteAndPersonaAsync(clienteId, personaId))
            throw new InvalidOperationException(
                "Esta persona ya está registrada como contacto de emergencia del cliente.");

        if (esPrincipal)
            await _contactRepository.DesmarcarPrincipalByClienteAsync(clienteId);

        var contact = EmergencyContact.Crear(clienteId, personaId, relacionId, esPrincipal);

        await _contactRepository.SaveAsync(contact);
        return contact;
    }
}
