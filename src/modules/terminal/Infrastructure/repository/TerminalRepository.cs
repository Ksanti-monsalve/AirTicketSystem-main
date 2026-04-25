// src/modules/terminal/Infrastructure/repository/TerminalRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.terminal.Domain.aggregate;
using AirTicketSystem.modules.terminal.Domain.Repositories;
using AirTicketSystem.modules.terminal.Infrastructure.entity;

namespace AirTicketSystem.modules.terminal.Infrastructure.repository;

public sealed class TerminalRepository : ITerminalRepository
{
    private readonly AppDbContext _context;

    public TerminalRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Terminal?> FindByIdAsync(int id)
    {
        var entity = await _context.Terminales
            .FirstOrDefaultAsync(t => t.Id == id);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Terminal>> FindByAeropuertoAsync(int aeropuertoId)
    {
        var entities = await _context.Terminales
            .Include(t => t.Puertas)
            .Where(t => t.AeropuertoId == aeropuertoId)
            .OrderBy(t => t.Nombre)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<bool> ExistsByNombreAndAeropuertoAsync(
        string nombre, int aeropuertoId)
        => await _context.Terminales
            .AnyAsync(t =>
                t.Nombre.ToLower() == nombre.ToLower() &&
                t.AeropuertoId == aeropuertoId);

    public async Task SaveAsync(Terminal terminal)
    {
        var entity = MapToEntity(terminal);
        await _context.Terminales.AddAsync(entity);
        await _context.SaveChangesAsync();
        terminal.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Terminal terminal)
    {
        var entity = await _context.Terminales.FindAsync(terminal.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontro la terminal con ID {terminal.Id} en la BD.");

        entity.Nombre = terminal.Nombre.Valor;
        entity.Descripcion = terminal.Descripcion?.Valor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Terminales.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontro la terminal con ID {id} en la BD.");

        _context.Terminales.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static Terminal MapToDomain(TerminalEntity entity)
        => Terminal.Reconstituir(
            entity.Id,
            entity.AeropuertoId,
            entity.Nombre,
            entity.Descripcion);

    private static TerminalEntity MapToEntity(Terminal terminal)
        => new()
        {
            AeropuertoId = terminal.AeropuertoId,
            Nombre = terminal.Nombre.Valor,
            Descripcion = terminal.Descripcion?.Valor
        };
}