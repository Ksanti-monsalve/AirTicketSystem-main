using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.modules.booking.Domain.aggregate;
using AirTicketSystem.modules.bookingpassenger.Application.UseCases;
using AirTicketSystem.modules.seat.Application.UseCases;
using AirTicketSystem.shared.helpers;

namespace AirTicketSystem.shared.UI;

/// <summary>Flujo único de «Seleccionar asiento en reserva» (cliente o admin) para criterio de integración del examen.</summary>
public static class ExamenAsientoReservaUI
{
    public static async Task EjecutarSeleccionAsientoEnReservaAsync(
        IServiceProvider provider,
        Booking reserva,
        CancellationToken cancellationToken = default)
    {
        await using var scope = provider.CreateAsyncScope();
        var sp = scope.ServiceProvider;

        var pasajeros = await sp
            .GetRequiredService<GetPassengersByBookingUseCase>()
            .ExecuteAsync(reserva.Id, cancellationToken);
        if (pasajeros.Count == 0)
            throw new InvalidOperationException("La reserva no tiene pasajeros.");

        var pasajero = SpectreHelper.SeleccionarOpcion(
            "Seleccione el pasajero",
            pasajeros,
            p => $"  Pasajero reserva #{p.Id}  persona {p.PersonaId}  tipo: {p.TipoPasajero.Valor}  " +
                $"asiento: {(p.AsientoId?.ToString() ?? "sin asignar")}");

        if (pasajero.AsientoId.HasValue)
            throw new InvalidOperationException(
                "Ese pasajero ya tiene asiento. Use «Cambiar asiento» o el menú de pasajeros (admin).");

        var vueloId = reserva.VueloId;

        var clases = await sp
            .GetRequiredService<GetAvailableFlightClassesByFlightUseCase>()
            .ExecuteAsync(vueloId, cancellationToken);
        if (clases.Count == 0)
            throw new InvalidOperationException("No hay clases con asientos libres en ese vuelo.");

        var claseSel = SpectreHelper.SeleccionarOpcion(
            "Clase de vuelo",
            clases,
            c => $"  [{c.FlightClassCode}] {c.FlightClassName}  ({c.Available} libres)");

        var asientos = await sp
            .GetRequiredService<GetAvailableSeatsByFlightAndClassUseCase>()
            .ExecuteAsync(vueloId, claseSel.FlightClassId, cancellationToken);
        if (asientos.Count == 0)
            throw new InvalidOperationException("No hay asientos libres en esa clase.");

        var asientoSel = SpectreHelper.SeleccionarOpcion(
            "Asiento (n.º de fila y columna, p. ej. 10A)",
            asientos,
            s => $"  {s.SeatNumber}  —  {s.FlightClassName}");

        await sp.GetRequiredService<SelectSeatForPassengerUseCase>()
            .SelectAsync(
                pasajeroReservaId: pasajero.Id,
                vueloId: vueloId,
                flightClassId: claseSel.FlightClassId,
                seatNumber: asientoSel.SeatNumber,
                joinAmbientTransaction: false,
                cancellationToken);
    }
}
