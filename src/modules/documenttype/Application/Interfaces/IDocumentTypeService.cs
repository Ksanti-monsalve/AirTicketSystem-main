// src/modules/documenttype/Application/Interfaces/IDocumentTypeService.cs
using AirTicketSystem.modules.documenttype.Domain.aggregate;

namespace AirTicketSystem.modules.documenttype.Application.Interfaces;

public interface IDocumentTypeService
{
    Task<DocumentType> CreateAsync(string descripcion);
    Task<DocumentType> GetByIdAsync(int id);
    Task<IReadOnlyCollection<DocumentType>> GetAllAsync();
    Task<DocumentType> UpdateAsync(int id, string descripcion);
    Task DeleteAsync(int id);
}
