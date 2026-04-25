// src/modules/documenttype/Domain/Repositories/IDocumentTypeRepository.cs
using AirTicketSystem.modules.documenttype.Domain.aggregate;

namespace AirTicketSystem.modules.documenttype.Domain.Repositories;

public interface IDocumentTypeRepository
{
    Task<DocumentType?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<DocumentType>> FindAllAsync();
    Task<bool> ExistsByDescripcionAsync(string descripcion);
    Task SaveAsync(DocumentType documentType);
    Task UpdateAsync(DocumentType documentType);
    Task DeleteAsync(int id);
}
