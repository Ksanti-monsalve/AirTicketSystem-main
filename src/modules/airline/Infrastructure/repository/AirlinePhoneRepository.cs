// src/modules/airline/Infrastructure/repository/AirlinePhoneRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.airline.Domain.aggregate;
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airline.Infrastructure.entity;

namespace AirTicketSystem.modules.airline.Infrastructure.repository;

public sealed class AirlinePhoneRepository : IAirlinePhoneRepository
{
    private readonly AppDbContext _context;

    public AirlinePhoneRepository(AppDbContext context) => _context = context;

    public async Task<AirlinePhone?> FindByIdAsync(int id)
    {
        var entity = await _context.TelefonosAerolinea.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<AirlinePhone>> FindByAerolineaAsync(int aerolineaId)
    {
        var entities = await _context.TelefonosAerolinea
            .Where(p => p.AerolineaId == aerolineaId)
            .OrderByDescending(p => p.EsPrincipal)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<AirlinePhone?> FindPrincipalByAerolineaAsync(int aerolineaId)
    {
        var entity = await _context.TelefonosAerolinea
            .FirstOrDefaultAsync(p =>
                p.AerolineaId == aerolineaId && p.EsPrincipal);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<bool> ExistsByNumeroAndAerolineaAsync(string numero, int aerolineaId)
        => await _context.TelefonosAerolinea
            .AnyAsync(p =>
                p.Numero == numero &&
                p.AerolineaId == aerolineaId);

    public async Task DesmarcarPrincipalByAerolineaAsync(int aerolineaId)
    {
        var telefonos = await _context.TelefonosAerolinea
            .Where(p => p.AerolineaId == aerolineaId && p.EsPrincipal)
            .ToListAsync();

        foreach (var t in telefonos)
            t.EsPrincipal = false;

        await _context.SaveChangesAsync();
    }

    public async Task SaveAsync(AirlinePhone phone)
    {
        var entity = MapToEntity(phone);
        _context.TelefonosAerolinea.Add(entity);
        await _context.SaveChangesAsync();
        phone.EstablecerId(entity.Id);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.TelefonosAerolinea.FindAsync(id);
        if (entity is not null)
        {
            _context.TelefonosAerolinea.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    private static AirlinePhone MapToDomain(AirlinePhoneEntity e) =>
        AirlinePhone.Reconstituir(
            e.Id,
            e.AerolineaId,
            e.TipoTelefonoId,
            e.Numero,
            e.IndicativoPais,
            e.EsPrincipal);

    private static AirlinePhoneEntity MapToEntity(AirlinePhone ap) => new()
    {
        AerolineaId    = ap.AerolineaId,
        TipoTelefonoId = ap.TipoTelefonoId,
        Numero         = ap.Numero.Valor,
        IndicativoPais = ap.IndicativoPais?.Valor,
        EsPrincipal    = ap.EsPrincipal.Valor
    };
}
