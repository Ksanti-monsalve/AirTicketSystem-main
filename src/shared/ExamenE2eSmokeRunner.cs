// src/shared/ExamenE2eSmokeRunner.cs
// Reproduce en código el flujo: vuelo nuevo + reserva + pasajero → asignar asiento (menús 2) → consulta por reserva (9 / 11).
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.modules.booking.Application.UseCases;
using AirTicketSystem.modules.bookingpassenger.Application.UseCases;
using AirTicketSystem.modules.flight.Application.UseCases;
using AirTicketSystem.modules.flight.Domain.aggregate;
using AirTicketSystem.modules.seat.Application.UseCases;
using AirTicketSystem.shared.context;

namespace AirTicketSystem.shared;

public static class ExamenE2eSmokeRunner
{
    public static async Task<int> RunAsync(IServiceProvider rootProvider, CancellationToken ct = default)
    {
        static void Out(string s)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  " + s);
            Console.ResetColor();
        }

        static void Ok(string s)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ✓ " + s);
            Console.ResetColor();
        }

        static int Fail(string s)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  ✗ " + s);
            Console.ResetColor();
            return 2;
        }

        await using var scope = rootProvider.CreateAsyncScope();
        var sp   = scope.ServiceProvider;
        var db   = sp.GetRequiredService<AppDbContext>();

        var ruta = await db.Rutas.AsNoTracking()
            .FirstOrDefaultAsync(r => r.Activa, ct);
        if (ruta is null) return Fail("No hay ruta activa. Ejecute el seed o cree rutas.");

        var avion = await db.Aviones.AsNoTracking()
            .FirstOrDefaultAsync(a => a.Estado == "DISPONIBLE", ct);
        if (avion is null) return Fail("No hay avión en estado DISPONIBLE.");

        var tarifaE = await db.Tarifas.AsNoTracking()
            .FirstOrDefaultAsync(
                f => f.RutaId == ruta.Id && f.Activa, ct);
        if (tarifaE is null) return Fail("No hay tarifa activa para la primera ruta activa.");

        var clienteE = await db.Clientes.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Activo, ct);
        if (clienteE is null) return Fail("No hay clientes activos.");

        var personaE = await db.Personas.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == clienteE.PersonaId, ct);
        if (personaE is null) return Fail("El cliente de prueba no tiene persona vinculada.");

        // NumeroVueloFlight: longitud 3–6
        var numeroVuelo = $"E2E{DateTime.UtcNow.Millisecond % 1000:000}";
        var salida  = DateTime.UtcNow.AddDays(9).Date.AddHours(8);
        var llegada = salida.AddHours(2);

        Out($"Creando vuelo {numeroVuelo} (ruta {ruta.Id}, avión {avion.Id})…");
        var createFlight = sp.GetRequiredService<CreateFlightUseCase>();
        Flight flight;
        try
        {
            flight = await createFlight.ExecuteAsync(
                ruta.Id, avion.Id, numeroVuelo, salida, llegada, null, ct);
        }
        catch (Exception ex)
        {
            return Fail("CreateFlight: " + ex.Message);
        }
        Ok($"Vuelo creado id={flight.Id} (mapa de asientos generado).");

        Out("Creando reserva + pasajero adulto sin asiento…");
        var createBooking = sp.GetRequiredService<CreateBookingUseCase>();
        var booking = await createBooking.ExecuteAsync(
            clienteE.Id, flight.Id, tarifaE.Id, tarifaE.PrecioTotal, "E2E examen", ct);

        var addPax = sp.GetRequiredService<AddPassengerUseCase>();
        var pax = await addPax.ExecuteAsync(booking.Id, personaE.Id, "ADULTO", null, ct);
        Ok($"Reserva {booking.Id} / pasajero {pax.Id} sin asiento.");

        // Equivale a menú «2. Seleccionar asiento en reserva»
        Out("Seleccionando asiento (SelectSeatForPassenger)…");
        var clasesUc = sp.GetRequiredService<GetAvailableFlightClassesByFlightUseCase>();
        var clases   = await clasesUc.ExecuteAsync(flight.Id, ct);
        if (clases.Count == 0) return Fail("No hay clases con asientos libres en el vuelo.");
        var cls = clases.First();

        var seatsUc = sp.GetRequiredService<GetAvailableSeatsByFlightAndClassUseCase>();
        var asientosDisponibles = await seatsUc.ExecuteAsync(flight.Id, cls.FlightClassId, ct);
        if (asientosDisponibles.Count == 0) return Fail("No hay asientos libres en la primera clase con cupo.");

        var firstSeat = asientosDisponibles.First();
        var select    = sp.GetRequiredService<SelectSeatForPassengerUseCase>();
        try
        {
            await select.SelectAsync(
                pax.Id, flight.Id, cls.FlightClassId, firstSeat.SeatNumber,
                joinAmbientTransaction: false, cancellationToken: ct);
        }
        catch (Exception ex)
        {
            return Fail("SelectSeat: " + ex.Message);
        }
        Ok($"Asiento reservado: {firstSeat.SeatNumber} (clase {cls.FlightClassName}).");

        // Equivale a menús «9» (admin) y «11» (cliente): asientos de la reserva
        Out("Consultando asientos de la reserva (GetSeatsByBooking)…");
        var byReserva = sp.GetRequiredService<GetSeatsByBookingUseCase>();
        var mapa = await byReserva.ExecuteAsync(booking.Id, ct);
        if (mapa.Count == 0) return Fail("GetSeatsByBooking devolvió 0 filas.");
        foreach (var row in mapa)
        {
            Ok($"  mapa seats: {row.SeatNumber}  {row.FlightClassName}  {row.Status}  reserva {row.BookingId}");
        }

        Console.WriteLine();
        Ok($"E2E examen listo. Vuelo {flight.Id}  Reserva {booking.Id}  Código {booking.CodigoReserva.Valor}");
        return 0;
    }
}
