// src/modules/booking/Application/Services/BookingService.cs
using AirTicketSystem.modules.booking.Application.Interfaces;
using AirTicketSystem.modules.booking.Application.UseCases;
using AirTicketSystem.modules.booking.Domain.aggregate;

namespace AirTicketSystem.modules.booking.Application.Services;

public sealed class BookingService : IBookingService
{
    private readonly CreateBookingUseCase             _create;
    private readonly GetBookingByIdUseCase            _getById;
    private readonly GetBookingByCodigoUseCase        _getByCodigo;
    private readonly GetBookingsByClienteUseCase      _getByCliente;
    private readonly ConfirmBookingUseCase            _confirm;
    private readonly CancelBookingUseCase             _cancel;
    private readonly ExpireBookingUseCase             _expire;
    private readonly ExtendBookingUseCase             _extend;
    private readonly UpdateBookingObservationsUseCase _updateObs;
    private readonly DeleteBookingUseCase             _delete;

    public BookingService(
        CreateBookingUseCase             create,
        GetBookingByIdUseCase            getById,
        GetBookingByCodigoUseCase        getByCodigo,
        GetBookingsByClienteUseCase      getByCliente,
        ConfirmBookingUseCase            confirm,
        CancelBookingUseCase             cancel,
        ExpireBookingUseCase             expire,
        ExtendBookingUseCase             extend,
        UpdateBookingObservationsUseCase updateObs,
        DeleteBookingUseCase             delete)
    {
        _create       = create;
        _getById      = getById;
        _getByCodigo  = getByCodigo;
        _getByCliente = getByCliente;
        _confirm      = confirm;
        _cancel       = cancel;
        _expire       = expire;
        _extend       = extend;
        _updateObs    = updateObs;
        _delete       = delete;
    }

    public Task<Booking> CreateAsync(
        int clienteId, int vueloId, int tarifaId,
        decimal valorTotal, string? observaciones)
        => _create.ExecuteAsync(clienteId, vueloId, tarifaId, valorTotal, observaciones);

    public Task<Booking> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<Booking> GetByCodigoAsync(string codigoReserva)
        => _getByCodigo.ExecuteAsync(codigoReserva);

    public Task<IReadOnlyCollection<Booking>> GetByClienteAsync(int clienteId)
        => _getByCliente.ExecuteAsync(clienteId);

    public Task<Booking> ConfirmAsync(int id, int? usuarioId)
        => _confirm.ExecuteAsync(id, usuarioId);

    public Task<Booking> CancelAsync(int id, string motivo, int? usuarioId)
        => _cancel.ExecuteAsync(id, motivo, usuarioId);

    public Task<Booking> ExpireAsync(int id)
        => _expire.ExecuteAsync(id);

    public Task<Booking> ExtendAsync(int id, int horas)
        => _extend.ExecuteAsync(id, horas);

    public Task<Booking> UpdateObservationsAsync(int id, string? observaciones)
        => _updateObs.ExecuteAsync(id, observaciones);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}
