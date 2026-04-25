// src/modules/ticket/Application/Services/TicketService.cs
using AirTicketSystem.modules.ticket.Application.Interfaces;
using AirTicketSystem.modules.ticket.Application.UseCases;
using AirTicketSystem.modules.ticket.Domain.aggregate;

namespace AirTicketSystem.modules.ticket.Application.Services;

public sealed class TicketService : ITicketService
{
    private readonly EmitTicketUseCase       _emit;
    private readonly GetTicketByCodeUseCase  _getByCode;
    private readonly CheckInTicketUseCase    _checkIn;
    private readonly BoardTicketUseCase      _board;
    private readonly VoidTicketUseCase       _void;

    public TicketService(
        EmitTicketUseCase      emit,
        GetTicketByCodeUseCase getByCode,
        CheckInTicketUseCase   checkIn,
        BoardTicketUseCase     board,
        VoidTicketUseCase      voidUseCase)
    {
        _emit      = emit;
        _getByCode = getByCode;
        _checkIn   = checkIn;
        _board     = board;
        _void      = voidUseCase;
    }

    public Task<Ticket> EmitAsync(int pasajeroReservaId, int? asientoConfirmadoId)
        => _emit.ExecuteAsync(pasajeroReservaId, asientoConfirmadoId);

    public Task<Ticket> GetByCodeAsync(string codigoTiquete)
        => _getByCode.ExecuteAsync(codigoTiquete);

    public Task<Ticket> CheckInAsync(int id, int? asientoConfirmadoId)
        => _checkIn.ExecuteAsync(id, asientoConfirmadoId);

    public Task<Ticket> BoardAsync(int id)
        => _board.ExecuteAsync(id);

    public Task<Ticket> VoidAsync(int id)
        => _void.ExecuteAsync(id);
}
