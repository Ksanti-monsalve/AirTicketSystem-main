// src/modules/seatavailability/Application/UseCases/BlockSeatUseCase.cs
using AirTicketSystem.modules.seatavailability.Domain.aggregate;
using AirTicketSystem.modules.seatavailability.Domain.Repositories;

namespace AirTicketSystem.modules.seatavailability.Application.UseCases;

public sealed class BlockSeatUseCase
{
    private readonly ISeatAvailabilityRepository _repository;

    public BlockSeatUseCase(ISeatAvailabilityRepository repository) => _repository = repository;

    public async Task<SeatAvailability> ExecuteAsync(
        int vueloId, int asientoId, CancellationToken cancellationToken = default)
    {
        var seat = await _repository.FindByVueloAndAsientoAsync(vueloId, asientoId)
            ?? throw new KeyNotFoundException(
                $"No se encontró disponibilidad para el asiento {asientoId} " +
                $"en el vuelo {vueloId}.");

        seat.Bloquear();
        await _repository.UpdateAsync(seat);
        return seat;
    }
}
