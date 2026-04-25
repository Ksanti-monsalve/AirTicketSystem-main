// src/modules/documenttype/Application/UseCases/CreateDocumentTypeUseCase.cs
using AirTicketSystem.modules.documenttype.Domain.aggregate;
using AirTicketSystem.modules.documenttype.Domain.Repositories;
using AirTicketSystem.modules.documenttype.Domain.ValueObjects;

namespace AirTicketSystem.modules.documenttype.Application.UseCases;

public sealed class CreateDocumentTypeUseCase
{
    private readonly IDocumentTypeRepository _repository;

    public CreateDocumentTypeUseCase(IDocumentTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<DocumentType> ExecuteAsync(
        string descripcion,
        CancellationToken cancellationToken = default)
    {
        var descripcionVO = DescripcionDocumentType.Crear(descripcion);

        if (await _repository.ExistsByDescripcionAsync(descripcionVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un tipo de documento con la descripción '{descripcionVO.Valor}'.");

        var documentType = DocumentType.Crear(descripcionVO.Valor);
        await _repository.SaveAsync(documentType);
        return documentType;
    }
}
