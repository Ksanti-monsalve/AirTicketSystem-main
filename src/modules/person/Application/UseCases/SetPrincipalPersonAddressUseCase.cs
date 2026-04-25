// src/modules/person/Application/UseCases/SetPrincipalPersonAddressUseCase.cs
using AirTicketSystem.modules.person.Domain.aggregate;
using AirTicketSystem.modules.person.Domain.Repositories;

namespace AirTicketSystem.modules.person.Application.UseCases;

public sealed class SetPrincipalPersonAddressUseCase
{
    private readonly IPersonAddressRepository _repository;

    public SetPrincipalPersonAddressUseCase(IPersonAddressRepository repository)
        => _repository = repository;

    public async Task<PersonAddress> ExecuteAsync(
        int addressId, CancellationToken cancellationToken = default)
    {
        if (addressId <= 0)
            throw new ArgumentException("El ID de la dirección no es válido.");

        var address = await _repository.FindByIdAsync(addressId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una dirección con ID {addressId}.");

        await _repository.DesmarcarPrincipalByPersonaAsync(address.PersonaId);

        address.MarcarComoPrincipal();
        await _repository.UpdateAsync(address);
        return address;
    }
}
