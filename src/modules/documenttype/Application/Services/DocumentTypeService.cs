// src/modules/documenttype/Application/Services/DocumentTypeService.cs
using AirTicketSystem.modules.documenttype.Application.Interfaces;
using AirTicketSystem.modules.documenttype.Application.UseCases;
using AirTicketSystem.modules.documenttype.Domain.aggregate;

namespace AirTicketSystem.modules.documenttype.Application.Services;

public sealed class DocumentTypeService : IDocumentTypeService
{
    private readonly CreateDocumentTypeUseCase _create;
    private readonly GetDocumentTypeByIdUseCase _getById;
    private readonly GetAllDocumentTypesUseCase _getAll;
    private readonly UpdateDocumentTypeUseCase _update;
    private readonly DeleteDocumentTypeUseCase _delete;

    public DocumentTypeService(
        CreateDocumentTypeUseCase create,
        GetDocumentTypeByIdUseCase getById,
        GetAllDocumentTypesUseCase getAll,
        UpdateDocumentTypeUseCase update,
        DeleteDocumentTypeUseCase delete)
    {
        _create  = create;
        _getById = getById;
        _getAll  = getAll;
        _update  = update;
        _delete  = delete;
    }

    public Task<DocumentType> CreateAsync(string descripcion)
        => _create.ExecuteAsync(descripcion);

    public Task<DocumentType> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<DocumentType>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<DocumentType> UpdateAsync(int id, string descripcion)
        => _update.ExecuteAsync(id, descripcion);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}
