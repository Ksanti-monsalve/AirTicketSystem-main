// src/modules/user/Infrastructure/repository/AccessLogRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.user.Domain.aggregate;
using AirTicketSystem.modules.user.Domain.Repositories;
using AirTicketSystem.modules.user.Infrastructure.entity;

namespace AirTicketSystem.modules.user.Infrastructure.repository;

public sealed class AccessLogRepository : IAccessLogRepository
{
    private readonly AppDbContext _context;

    public AccessLogRepository(AppDbContext context) => _context = context;

    public async Task<AccessLog?> FindByIdAsync(int id)
    {
        var entity = await _context.LogsAcceso.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<AccessLog>> FindByUsuarioAsync(int usuarioId)
    {
        var entities = await _context.LogsAcceso
            .Where(l => l.UsuarioId == usuarioId)
            .OrderByDescending(l => l.FechaAcceso)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<AccessLog>> FindByUsuarioAndTipoAsync(
        int usuarioId, string tipo)
    {
        var entities = await _context.LogsAcceso
            .Where(l =>
                l.UsuarioId == usuarioId &&
                l.Tipo == tipo.ToUpperInvariant())
            .OrderByDescending(l => l.FechaAcceso)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<AccessLog?> FindUltimoLoginAsync(int usuarioId)
    {
        var entity = await _context.LogsAcceso
            .Where(l => l.UsuarioId == usuarioId && l.Tipo == "LOGIN")
            .OrderByDescending(l => l.FechaAcceso)
            .FirstOrDefaultAsync();
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<int> ContarIntentosFallidosRecientesAsync(
        int usuarioId, DateTime desde)
        => await _context.LogsAcceso
            .CountAsync(l =>
                l.UsuarioId == usuarioId &&
                l.Tipo == "INTENTO_FALLIDO" &&
                l.FechaAcceso >= desde);

    public async Task SaveAsync(AccessLog log)
    {
        var entity = MapToEntity(log);
        _context.LogsAcceso.Add(entity);
        await _context.SaveChangesAsync();
        log.EstablecerId(entity.Id);
    }

    private static AccessLog MapToDomain(AccessLogEntity entity)
        => AccessLog.Reconstituir(
            entity.Id,
            entity.UsuarioId,
            entity.FechaAcceso,
            entity.Tipo,
            entity.IpAddress);

    private static AccessLogEntity MapToEntity(AccessLog log)
        => new()
        {
            UsuarioId   = log.UsuarioId,
            FechaAcceso = log.FechaAcceso.Valor,
            Tipo        = log.Tipo.Valor,
            IpAddress   = log.IpAddress?.Valor
        };
}
