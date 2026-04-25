// src/modules/boardingpass/Infrastructure/repository/BoardingPassRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.boardingpass.Domain.aggregate;
using AirTicketSystem.modules.boardingpass.Domain.Repositories;
using AirTicketSystem.modules.boardingpass.Infrastructure.entity;

namespace AirTicketSystem.modules.boardingpass.Infrastructure.repository;

public sealed class BoardingPassRepository : IBoardingPassRepository
{
    private readonly AppDbContext _context;

    public BoardingPassRepository(AppDbContext context) => _context = context;

    public async Task<BoardingPass?> FindByIdAsync(int id)
    {
        var entity = await _context.PasesAbordar.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<BoardingPass?> FindByCodigoPaseAsync(string codigoPase)
    {
        var entity = await _context.PasesAbordar
            .FirstOrDefaultAsync(bp => bp.CodigoPase == codigoPase.ToUpperInvariant());
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<BoardingPass?> FindByCheckinAsync(int checkinId)
    {
        var entity = await _context.PasesAbordar
            .FirstOrDefaultAsync(bp => bp.CheckinId == checkinId);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<BoardingPass>> FindByPuertaAsync(int puertaEmbarqueId)
    {
        var entities = await _context.PasesAbordar
            .Where(bp => bp.PuertaEmbarqueId == puertaEmbarqueId)
            .OrderBy(bp => bp.HoraEmbarque)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<bool> ExistsByCodigoPaseAsync(string codigoPase)
        => await _context.PasesAbordar
            .AnyAsync(bp => bp.CodigoPase == codigoPase.ToUpperInvariant());

    public async Task<bool> ExistsByCheckinAsync(int checkinId)
        => await _context.PasesAbordar
            .AnyAsync(bp => bp.CheckinId == checkinId);

    public async Task SaveAsync(BoardingPass boardingPass)
    {
        var entity = MapToEntity(boardingPass);
        _context.PasesAbordar.Add(entity);
        await _context.SaveChangesAsync();
        boardingPass.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(BoardingPass boardingPass)
    {
        var entity = await _context.PasesAbordar.FindAsync(boardingPass.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró el pase de abordar con ID {boardingPass.Id}.");

        entity.PuertaEmbarqueId = boardingPass.PuertaEmbarqueId;
        entity.HoraEmbarque     = boardingPass.HoraEmbarque?.Valor;

        await _context.SaveChangesAsync();
    }

    private static BoardingPass MapToDomain(BoardingPassEntity e) =>
        BoardingPass.Reconstituir(
            e.Id,
            e.CheckinId,
            e.CodigoPase,
            e.CodigoQr,
            e.PuertaEmbarqueId,
            e.HoraEmbarque,
            e.FechaEmision);

    private static BoardingPassEntity MapToEntity(BoardingPass bp) => new()
    {
        CheckinId        = bp.CheckinId,
        CodigoPase       = bp.CodigoPase.Valor,
        CodigoQr         = bp.CodigoQr.Valor,
        PuertaEmbarqueId = bp.PuertaEmbarqueId,
        HoraEmbarque     = bp.HoraEmbarque?.Valor,
        FechaEmision     = bp.FechaEmision.Valor
    };
}
