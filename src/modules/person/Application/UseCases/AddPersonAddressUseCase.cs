// src/modules/person/Application/UseCases/AddPersonAddressUseCase.cs
using AirTicketSystem.modules.person.Domain.aggregate;
using AirTicketSystem.modules.person.Domain.Repositories;

namespace AirTicketSystem.modules.person.Application.UseCases;

public sealed class AddPersonAddressUseCase
{
    private readonly IPersonRepository _personRepository;
    private readonly IPersonAddressRepository _addressRepository;

    public AddPersonAddressUseCase(
        IPersonRepository personRepository,
        IPersonAddressRepository addressRepository)
    {
        _personRepository  = personRepository;
        _addressRepository = addressRepository;
    }

    public async Task<PersonAddress> ExecuteAsync(
        int personaId,
        int tipoDireccionId,
        int ciudadId,
        string direccionLinea1,
        string? direccionLinea2 = null,
        string? codigoPostal = null,
        bool esPrincipal = false,
        CancellationToken cancellationToken = default)
    {
        _ = await _personRepository.FindByIdAsync(personaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una persona con ID {personaId}.");

        if (esPrincipal)
            await _addressRepository.DesmarcarPrincipalByPersonaAsync(personaId);

        var address = PersonAddress.Crear(
            personaId, tipoDireccionId, ciudadId,
            direccionLinea1, direccionLinea2, codigoPostal, esPrincipal);

        await _addressRepository.SaveAsync(address);
        return address;
    }
}
