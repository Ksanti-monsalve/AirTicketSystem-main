using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.flight.Application.UseCases;
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.seat.Application.UseCases;
using AirTicketSystem.UI.Admin.Reservations;

namespace AirTicketSystem.UI.Admin.Flights;

/// <summary>
/// Enunciado examen (§5 y §6): opciones y textos alineados a
/// «Ver asientos por vuelo», «Seleccionar asiento en reserva», «Consultar disponibilidad por clase»,
/// «Ver asientos ocupados», «Cambiar asiento».
/// </summary>
public sealed class SeatClassAdminMenu
{
    private readonly IServiceProvider _provider;

    public SeatClassAdminMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("ADMIN — Selección de asientos y clases de vuelo (examen)");

            // Orden: configurar vuelo → inspección del mapa (disponible/ocupado/%) y reservas → operar asignaciones → ajuste de asiento.
            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "1. Crear vuelo (genera mapa de asientos)",
                    "2. Ver asientos por vuelo",
                    "3. Consultar disponibilidad por clase",
                    "4. Ver asientos ocupados",
                    "5. Ver porcentaje de ocupación",
                    "6. Consultar reservas",
                    "7. Seleccionar asiento en reserva",
                    "8. Asientos asignados a una reserva",
                    "9. Cambiar asiento (pasajeros)",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "1. Crear vuelo (genera mapa de asientos)":
                    await CrearVueloAsync();
                    break;
                case "2. Ver asientos por vuelo":
                    await VerAsientosPorVueloAsync();
                    break;
                case "3. Consultar disponibilidad por clase":
                    await VerDisponibilidadPorClaseAsync();
                    break;
                case "4. Ver asientos ocupados":
                    await VerAsientosOcupadosAsync();
                    break;
                case "5. Ver porcentaje de ocupación":
                    await VerPorcentajeOcupacionAsync();
                    break;
                case "6. Consultar reservas":
                    await ConsultarReservasAsync();
                    break;
                case "7. Seleccionar asiento en reserva":
                    await SeleccionarAsientoEnReservaAdminAsync();
                    break;
                case "8. Asientos asignados a una reserva":
                    await AsientosAsignadosAReservaAsync();
                    break;
                case "9. Cambiar asiento (pasajeros)":
                    await new PassengerMenu(_provider).MostrarAsync();
                    break;
                case "Volver":
                    return;
            }
        }
    }

    private async Task SeleccionarAsientoEnReservaAdminAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            var reserva = await SelectorUI.SeleccionarReservaCualquieraAsync(_provider);
            if (reserva is null) return;
            await ExamenAsientoReservaUI.EjecutarSeleccionAsientoEnReservaAsync(_provider, reserva);
            SpectreHelper.MostrarExito("Asiento reservado; estado: Reservado (MySQL, transacción atómica).");
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task AsientosAsignadosAReservaAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            var reserva = await SelectorUI.SeleccionarReservaCualquieraAsync(_provider);
            if (reserva is null) return;
            await using var scope = _provider.CreateAsyncScope();
            var asientos = await scope.ServiceProvider
                .GetRequiredService<GetSeatsByBookingUseCase>()
                .ExecuteAsync(reserva.Id);
            if (asientos.Count == 0)
            {
                SpectreHelper.MostrarInfo("Esta reserva no tiene asientos asignados en el mapa del vuelo (tabla seats).");
                SpectreHelper.EsperarTecla();
                return;
            }

            var tabla = SpectreHelper.CrearTabla("N.º asiento", "Clase", "Estado", "ReservaId");
            foreach (var s in asientos.OrderBy(x => x.SeatNumber))
            {
                SpectreHelper.AgregarFila(tabla,
                    s.SeatNumber,
                    s.FlightClassName,
                    SpectreHelper.FormatearEstadoAsientoExamen(s.Status),
                    s.BookingId?.ToString() ?? "—");
            }
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.MostrarLeyendaEstadosAsientoExamen();
            SpectreHelper.MostrarInfo($"Reserva [{reserva.CodigoReserva.Valor}]  vuelo {reserva.VueloId}  asientos: {asientos.Count}");
            SpectreHelper.EsperarTecla();
        });
    }

    private static async Task<int> ResolverVueloIdExamenAsync(
        IServiceProvider sp, string raw)
    {
        var trimmed = raw.Trim();
        if (string.IsNullOrEmpty(trimmed))
            throw new ArgumentException("Indique un ID o número de vuelo.");

        if (int.TryParse(trimmed, out var id) && id > 0)
        {
            var flight = await sp.GetRequiredService<IFlightRepository>().FindByIdAsync(id)
                ?? throw new KeyNotFoundException($"No existe vuelo con ID {id}.");
            return flight.Id;
        }

        var num = trimmed.ToUpperInvariant();
        var list = (await sp.GetRequiredService<IFlightRepository>().FindProgramadosAsync())
            .Where(f => f.NumeroVuelo.Valor == num)
            .ToList();
        if (list.Count == 0)
            throw new KeyNotFoundException(
                $"No se encontró un vuelo programado con número «{num}».");

        if (list.Count > 1)
        {
            SpectreHelper.MostrarAdvertencia(
                "Hay varias fechas con ese número de vuelo; se toma el primero de la lista programada.");
        }

        return list[0].Id;
    }

    private async Task CrearVueloAsync()
    {
        SpectreHelper.MostrarInfo("Al guardar un vuelo se genera el mapa de asientos (enunciado §4).");
        SpectreHelper.EsperarTecla();
        await new FlightMenu(_provider).MostrarAsync();
    }

    /// <summary>§6: número, clase, estado; visualmente differenciados (Disponible/Reservado/Ocupado).</summary>
    private async Task VerAsientosPorVueloAsync()
    {
        var raw = SpectreHelper.PedirTexto("Vuelo: ID numérico o número (p. ej. HK4800)");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var vueloId = await ResolverVueloIdExamenAsync(scope.ServiceProvider, raw);
            var asientos = await scope.ServiceProvider
                .GetRequiredService<GetSeatDetailsByFlightUseCase>()
                .ExecuteAsync(vueloId);

            if (asientos.Count == 0)
            {
                SpectreHelper.MostrarInfo("No hay asientos registrados para este vuelo.");
                SpectreHelper.EsperarTecla();
                return;
            }

            foreach (var grupo in asientos
                         .GroupBy(s => new { s.FlightClassId, s.FlightClassName })
                         .OrderBy(g => g.Key.FlightClassName))
            {
                SpectreHelper.MostrarSubtitulo(
                    $"Clase: {grupo.Key.FlightClassName} (id {grupo.Key.FlightClassId})");
                var tabla = SpectreHelper.CrearTabla("ID asiento", "N.º asiento", "Estado");
                foreach (var s in grupo.OrderBy(x => x.SeatNumber))
                {
                    SpectreHelper.AgregarFila(tabla,
                        s.Id.ToString(),
                        s.SeatNumber,
                        SpectreHelper.FormatearEstadoAsientoExamen(s.Status));
                }
                SpectreHelper.MostrarTabla(tabla);
            }
            SpectreHelper.MostrarLeyendaEstadosAsientoExamen();
            SpectreHelper.MostrarInfo($"Total asientos: {asientos.Count}");
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task VerAsientosOcupadosAsync()
    {
        var raw = SpectreHelper.PedirTexto("Vuelo: ID numérico o número (p. ej. HK4800)");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var vueloId = await ResolverVueloIdExamenAsync(scope.ServiceProvider, raw);
            var ocupados = await scope.ServiceProvider
                .GetRequiredService<GetSeatDetailsByFlightUseCase>()
                .ExecuteAsync(vueloId, status: "Occupied");

            if (ocupados.Count == 0)
            {
                SpectreHelper.MostrarInfo("No hay asientos en estado Ocupado en este vuelo.");
                SpectreHelper.EsperarTecla();
                return;
            }

            var tabla = SpectreHelper.CrearTabla("ID asiento", "N.º asiento", "Clase", "Estado");
            foreach (var s in ocupados)
                SpectreHelper.AgregarFila(tabla,
                    s.Id.ToString(),
                    s.SeatNumber,
                    s.FlightClassName,
                    SpectreHelper.FormatearEstadoAsientoExamen(s.Status));
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.MostrarLeyendaEstadosAsientoExamen();
            SpectreHelper.MostrarInfo($"Total ocupados: {ocupados.Count}");
            SpectreHelper.EsperarTecla();
        });
    }

    private Task VerDisponibilidadPorClaseAsync()
    {
        var raw = SpectreHelper.PedirTexto("Vuelo: ID numérico o número (p. ej. HK4800)");
        return ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var vueloId = await ResolverVueloIdExamenAsync(scope.ServiceProvider, raw);
            var stats = await scope.ServiceProvider
                .GetRequiredService<GetSeatStatsByFlightUseCase>()
                .ExecuteAsync(vueloId);

            if (stats.Count == 0)
            {
                SpectreHelper.MostrarInfo("No hay asientos registrados para este vuelo.");
                SpectreHelper.EsperarTecla();
                return;
            }

            var tabla = SpectreHelper.CrearTabla(
                "Clase (id)", "Código", "Nombre",
                "Total", "Disponibles", "Reservados", "Ocupados", "Bloqueados");

            foreach (var s in stats)
                SpectreHelper.AgregarFila(tabla,
                    s.FlightClassId.ToString(),
                    s.FlightClassCode,
                    s.FlightClassName,
                    s.Total.ToString(),
                    s.Available.ToString(),
                    s.Reserved.ToString(),
                    s.Occupied.ToString(),
                    s.Blocked.ToString());

            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.MostrarInfo(
                "La tarifa (Fare) de la reserva define el importe; la clase de cabina define el tramo " +
                "de asiento en el vuelo.");
            SpectreHelper.EsperarTecla();
        });
    }

    private Task VerPorcentajeOcupacionAsync()
    {
        var raw = SpectreHelper.PedirTexto("Vuelo: ID numérico o número (p. ej. HK4800)");
        return ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var vueloId = await ResolverVueloIdExamenAsync(scope.ServiceProvider, raw);
            var stats = await scope.ServiceProvider
                .GetRequiredService<GetSeatStatsByFlightUseCase>()
                .ExecuteAsync(vueloId);

            if (stats.Count == 0)
            {
                SpectreHelper.MostrarInfo("No hay asientos registrados para este vuelo.");
                SpectreHelper.EsperarTecla();
                return;
            }

            var total = stats.Sum(s => s.Total);
            var ocupados = stats.Sum(s => s.Occupied);
            var porcentaje = total == 0 ? 0m : (decimal)ocupados * 100m / (decimal)total;

            var tabla = SpectreHelper.CrearTabla("Clase", "Ocupados", "Total", "% ocupación");
            foreach (var s in stats)
            {
                var pct = s.Total == 0 ? 0m : (decimal)s.Occupied * 100m / (decimal)s.Total;
                SpectreHelper.AgregarFila(tabla,
                    s.FlightClassName,
                    s.Occupied.ToString(),
                    s.Total.ToString(),
                    $"{pct:F2}%");
            }
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.MostrarInfo(
                $"Ocupación total vuelo {vueloId}: {ocupados}/{total} ({porcentaje:F2}%)");
            SpectreHelper.EsperarTecla();
        });
    }

    private Task ConsultarReservasAsync()
    {
        SpectreHelper.MostrarInfo("Módulo de reservas (admin).");
        SpectreHelper.EsperarTecla();
        return new BookingMenu(_provider, _provider.GetRequiredService<SessionContext>())
            .MostrarAsync();
    }
}
