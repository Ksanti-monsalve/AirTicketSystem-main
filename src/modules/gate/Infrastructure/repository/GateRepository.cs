// src/modules/gate/Infrastructure/repository/GateRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.gate.Domain.aggregate;
using AirTicketSystem.modules.gate.Domain.Repositories;
using AirTicketSystem.modules.gate.Infrastructure.entity;

namespace AirTicketSystem.modules.gate.Infrastructure.repository;

public sealed class GateRepository : IGateRepository
{
    private readonly AppDbContext _context;

    public GateRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Gate?> FindByIdAsync(int id)
    {
        var entity = await _context.PuertasEmbarque
            .FirstOrDefaultAsync(g => g.Id == id);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Gate>> FindByTerminalAsync(int terminalId)
    {
        var entities = await _context.PuertasEmbarque
            .Where(g => g.TerminalId == terminalId)
            .OrderBy(g => g.Codigo)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Gate>> FindActivasByTerminalAsync(int terminalId)
    {
        var entities = await _context.PuertasEmbarque
            .Where(g => g.TerminalId == terminalId && g.Activa)
            .OrderBy(g => g.Codigo)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<bool> ExistsByCodigoAndTerminalAsync(
        string codigo, int terminalId)
        => await _context.PuertasEmbarque
            .AnyAsync(g =>
                g.Codigo == codigo.ToUpperInvariant() &&
                g.TerminalId == terminalId);

    public async Task SaveAsync(Gate gate)
    {
        var entity = MapToEntity(gate);
        await _context.PuertasEmbarque.AddAsync(entity);
        await _context.SaveChangesAsync();
        gate.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Gate gate)
    {
        var entity = await _context.PuertasEmbarque.FindAsync(gate.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontro la puerta con ID {gate.Id} en la BD.");

        entity.Codigo = gate.Codigo.Valor;
        entity.Activa = gate.Activa.Valor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.PuertasEmbarque.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontro la puerta con ID {id} en la BD.");

        _context.PuertasEmbarque.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static Gate MapToDomain(GateEntity entity)
        => Gate.Reconstituir(
            entity.Id,
            entity.TerminalId,
            entity.Codigo,
            entity.Activa);

    private static GateEntity MapToEntity(Gate gate)
        => new()
        {
            TerminalId = gate.TerminalId,
            Codigo = gate.Codigo.Valor,
            Activa = gate.Activa.Valor
        };
}