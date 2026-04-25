// src/modules/contactrelationship/Application/UseCases/CreateContactRelationshipUseCase.cs
using AirTicketSystem.modules.contactrelationship.Domain.aggregate;
using AirTicketSystem.modules.contactrelationship.Domain.Repositories;
using AirTicketSystem.modules.contactrelationship.Domain.ValueObjects;

namespace AirTicketSystem.modules.contactrelationship.Application.UseCases;

public sealed class CreateContactRelationshipUseCase
{
    private readonly IContactRelationshipRepository _repository;

    public CreateContactRelationshipUseCase(IContactRelationshipRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContactRelationship> ExecuteAsync(
        string descripcion,
        CancellationToken cancellationToken = default)
    {
        var descripcionVO = DescripcionContactRelationship.Crear(descripcion);

        if (await _repository.ExistsByDescripcionAsync(descripcionVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe una relación de contacto con la descripción '{descripcionVO.Valor}'.");

        var contactRelationship = ContactRelationship.Crear(descripcionVO.Valor);
        await _repository.SaveAsync(contactRelationship);
        return contactRelationship;
    }
}
