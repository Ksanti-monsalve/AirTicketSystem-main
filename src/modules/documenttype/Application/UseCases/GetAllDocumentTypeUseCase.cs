// src/modules/documenttype/Application/UseCases/GetAllDocumentTypesUseCase.cs
using AirTicketSystem.modules.documenttype.Domain.aggregate;
using AirTicketSystem.modules.documenttype.Domain.Repositories;

namespace AirTicketSystem.modules.documenttype.Application.UseCases;

public sealed class GetAllDocumentTypesUseCase
{
    private readonly IDocumentTypeRepository _repository;

    public GetAllDocumentTypesUseCase(IDocumentTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<DocumentType>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return await _repository.FindAllAsync();
    }
}
