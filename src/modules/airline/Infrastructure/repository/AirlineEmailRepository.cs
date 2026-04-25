// src/modules/airline/Infrastructure/repository/AirlineEmailRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.airline.Domain.aggregate;
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airline.Infrastructure.entity;

namespace AirTicketSystem.modules.airline.Infrastructure.repository;

public sealed class AirlineEmailRepository : IAirlineEmailRepository
{
    private readonly AppDbContext _context;

    public AirlineEmailRepository(AppDbContext context) => _context = context;

    public async Task<AirlineEmail?> FindByIdAsync(int id)
    {
        var entity = await _context.EmailsAerolinea.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<AirlineEmail>> FindByAerolineaAsync(int aerolineaId)
    {
        var entities = await _context.EmailsAerolinea
            .Where(e => e.AerolineaId == aerolineaId)
            .OrderByDescending(e => e.EsPrincipal)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<AirlineEmail?> FindPrincipalByAerolineaAsync(int aerolineaId)
    {
        var entity = await _context.EmailsAerolinea
            .FirstOrDefaultAsync(e =>
                e.AerolineaId == aerolineaId && e.EsPrincipal);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<bool> ExistsByEmailAndAerolineaAsync(string email, int aerolineaId)
        => await _context.EmailsAerolinea
            .AnyAsync(e =>
                e.Email.ToLower() == email.ToLower() &&
                e.AerolineaId == aerolineaId);

    public async Task DesmarcarPrincipalByAerolineaAsync(int aerolineaId)
    {
        var emails = await _context.EmailsAerolinea
            .Where(e => e.AerolineaId == aerolineaId && e.EsPrincipal)
            .ToListAsync();

        foreach (var e in emails)
            e.EsPrincipal = false;

        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(AirlineEmail email)
    {
        var entity = MapToEntity(email);
        _context.EmailsAerolinea.Add(entity);
        await _context.SaveChangesAsync();
        email.EstablecerId(entity.Id);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.EmailsAerolinea.FindAsync(id);
        if (entity is not null)
        {
            _context.EmailsAerolinea.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    private static AirlineEmail MapToDomain(AirlineEmailEntity e) =>
        AirlineEmail.Reconstituir(
            e.Id,
            e.AerolineaId,
            e.TipoEmailId,
            e.Email,
            e.EsPrincipal);

    private static AirlineEmailEntity MapToEntity(AirlineEmail ae) => new()
    {
        AerolineaId = ae.AerolineaId,
        TipoEmailId = ae.TipoEmailId,
        Email       = ae.Email.Valor,
        EsPrincipal = ae.EsPrincipal.Valor
    };
}
