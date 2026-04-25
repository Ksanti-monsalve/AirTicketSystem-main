// src/modules/documenttype/Application/UseCases/GetDocumentTypeByIdUseCase.cs
using AirTicketSystem.modules.documenttype.Domain.aggregate;
using AirTicketSystem.modules.documenttype.Domain.Repositories;

namespace AirTicketSystem.modules.documenttype.Application.UseCases;

public sealed class GetDocumentTypeByIdUseCase
{
    private readonly IDocumentTypeRepository _repository;

    public GetDocumentTypeByIdUseCase(IDocumentTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<DocumentType> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del tipo de documento no es válido.");

        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de documento con ID {id}.");
    }
}
