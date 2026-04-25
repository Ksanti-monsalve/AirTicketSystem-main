// src/modules/seatavailability/Application/Services/SeatAvailabilityService.cs
using AirTicketSystem.modules.seatavailability.Application.Interfaces;
using AirTicketSystem.modules.seatavailability.Application.UseCases;
using AirTicketSystem.modules.seatavailability.Domain.aggregate;

namespace AirTicketSystem.modules.seatavailability.Application.Services;

public sealed class SeatAvailabilityService : ISeatAvailabilityService
{
    private readonly GetAvailableSeatsByFlightUseCase _getAvailable;
    private readonly ReserveSeatUseCase               _reserve;
    private readonly ReleaseSeatUseCase               _release;
    private readonly BlockSeatUseCase                 _block;

    public SeatAvailabilityService(
        GetAvailableSeatsByFlightUseCase getAvailable,
        ReserveSeatUseCase               reserve,
        ReleaseSeatUseCase               release,
        BlockSeatUseCase                 block)
    {
        _getAvailable = getAvailable;
        _reserve      = reserve;
        _release      = release;
        _block        = block;
    }

    public Task<IReadOnlyCollection<SeatAvailability>> GetAvailableByFlightAsync(int vueloId)
        => _getAvailable.ExecuteAsync(vueloId);

    public Task<SeatAvailability> ReserveAsync(int vueloId, int asientoId)
        => _reserve.ExecuteAsync(vueloId, asientoId);

    public Task<SeatAvailability> ReleaseAsync(int vueloId, int asientoId)
        => _release.ExecuteAsync(vueloId, asientoId);

    public Task<SeatAvailability> BlockAsync(int vueloId, int asientoId)
        => _block.ExecuteAsync(vueloId, asientoId);
}
