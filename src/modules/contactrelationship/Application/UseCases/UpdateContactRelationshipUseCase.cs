// src/modules/contactrelationship/Application/UseCases/UpdateContactRelationshipUseCase.cs
using AirTicketSystem.modules.contactrelationship.Domain.aggregate;
using AirTicketSystem.modules.contactrelationship.Domain.Repositories;
using AirTicketSystem.modules.contactrelationship.Domain.ValueObjects;

namespace AirTicketSystem.modules.contactrelationship.Application.UseCases;

public sealed class UpdateContactRelationshipUseCase
{
    private readonly IContactRelationshipRepository _repository;

    public UpdateContactRelationshipUseCase(IContactRelationshipRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContactRelationship> ExecuteAsync(
        int id,
        string descripcion,
        CancellationToken cancellationToken = default)
    {
        var contactRelationship = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una relación de contacto con ID {id}.");

        var descripcionVO = DescripcionContactRelationship.Crear(descripcion);

        if (!string.Equals(descripcionVO.Valor, contactRelationship.Descripcion.Valor, StringComparison.OrdinalIgnoreCase) &&
            await _repository.ExistsByDescripcionAsync(descripcionVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otra relación de contacto con la descripción '{descripcionVO.Valor}'.");

        contactRelationship.ActualizarDescripcion(descripcionVO.Valor);
        await _repository.UpdateAsync(contactRelationship);
        return contactRelationship;
    }
}
