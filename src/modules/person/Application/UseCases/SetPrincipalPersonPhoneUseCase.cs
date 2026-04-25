// src/modules/person/Application/UseCases/SetPrincipalPersonPhoneUseCase.cs
using AirTicketSystem.modules.person.Domain.aggregate;
using AirTicketSystem.modules.person.Domain.Repositories;

namespace AirTicketSystem.modules.person.Application.UseCases;

public sealed class SetPrincipalPersonPhoneUseCase
{
    private readonly IPersonPhoneRepository _repository;

    public SetPrincipalPersonPhoneUseCase(IPersonPhoneRepository repository)
        => _repository = repository;

    public async Task<PersonPhone> ExecuteAsync(
        int phoneId, CancellationToken cancellationToken = default)
    {
        if (phoneId <= 0)
            throw new ArgumentException("El ID del teléfono no es válido.");

        var phone = await _repository.FindByIdAsync(phoneId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un teléfono con ID {phoneId}.");

        await _repository.DesmarcarPrincipalByPersonaAsync(phone.PersonaId);

        phone.MarcarComoPrincipal();
        await _repository.UpdateAsync(phone);
        return phone;
    }
}
