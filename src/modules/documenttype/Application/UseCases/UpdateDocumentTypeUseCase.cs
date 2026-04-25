// src/modules/documenttype/Application/UseCases/UpdateDocumentTypeUseCase.cs
using AirTicketSystem.modules.documenttype.Domain.aggregate;
using AirTicketSystem.modules.documenttype.Domain.Repositories;
using AirTicketSystem.modules.documenttype.Domain.ValueObjects;

namespace AirTicketSystem.modules.documenttype.Application.UseCases;

public sealed class UpdateDocumentTypeUseCase
{
    private readonly IDocumentTypeRepository _repository;

    public UpdateDocumentTypeUseCase(IDocumentTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<DocumentType> ExecuteAsync(
        int id,
        string descripcion,
        CancellationToken cancellationToken = default)
    {
        var documentType = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de documento con ID {id}.");

        var descripcionVO = DescripcionDocumentType.Crear(descripcion);

        if (!string.Equals(descripcionVO.Valor, documentType.Descripcion.Valor, StringComparison.OrdinalIgnoreCase) &&
            await _repository.ExistsByDescripcionAsync(descripcionVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otro tipo de documento con la descripción '{descripcionVO.Valor}'.");

        documentType.ActualizarDescripcion(descripcionVO.Valor);
        await _repository.UpdateAsync(documentType);
        return documentType;
    }
}
