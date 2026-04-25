// src/modules/documenttype/Infrastructure/repository/DocumentTypeRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.documenttype.Domain.aggregate;
using AirTicketSystem.modules.documenttype.Domain.Repositories;
using AirTicketSystem.modules.documenttype.Infrastructure.entity;

namespace AirTicketSystem.modules.documenttype.Infrastructure.repository;

public sealed class DocumentTypeRepository : IDocumentTypeRepository
{
    private readonly AppDbContext _context;

    public DocumentTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DocumentType?> FindByIdAsync(int id)
    {
        var entity = await _context.TiposDocumento.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<DocumentType>> FindAllAsync()
    {
        var entities = await _context.TiposDocumento.OrderBy(d => d.Descripcion).ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<bool> ExistsByDescripcionAsync(string descripcion)
        => await _context.TiposDocumento
            .AnyAsync(d => d.Descripcion.ToLower() == descripcion.ToLower());

    public async Task SaveAsync(DocumentType documentType)
    {
        var entity = MapToEntity(documentType);
        await _context.TiposDocumento.AddAsync(entity);
        await _context.SaveChangesAsync();
        documentType.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(DocumentType documentType)
    {
        var entity = await _context.TiposDocumento.FindAsync(documentType.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró el tipo de documento con ID {documentType.Id} en la BD.");

        entity.Descripcion = documentType.Descripcion.Valor;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.TiposDocumento.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró el tipo de documento con ID {id} en la BD.");

        _context.TiposDocumento.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static DocumentType MapToDomain(DocumentTypeEntity entity)
        => DocumentType.Reconstituir(entity.Id, entity.Descripcion);

    private static DocumentTypeEntity MapToEntity(DocumentType documentType)
        => new() { Descripcion = documentType.Descripcion.Valor };
}
