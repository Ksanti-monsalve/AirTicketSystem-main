using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.seat.Application.UseCases;
using AirTicketSystem.modules.booking.Application.UseCases;
using AirTicketSystem.modules.bookingpassenger.Application.UseCases;
using AirTicketSystem.modules.client.Application.UseCases;

namespace AirTicketSystem.UI.Client;

/// <summary>
/// Enunciado examen (§5): «Ver asientos por vuelo», «Seleccionar asiento en reserva»,
/// «Consultar disponibilidad por clase», «Ver asientos ocupados», «Cambiar asiento».
/// </summary>
public sealed class SeatSelectionClientMenu
{
    private readonly IServiceProvider _provider;
    private readonly SessionContext   _session;

    private int? _vueloIdSeleccionado;
    private int? _flightClassSeleccionadaId;

    public SeatSelectionClientMenu(IServiceProvider provider, SessionContext session)
    {
        _provider = provider;
        _session  = session;
    }

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("CLIENTE — Selección de asientos y clases de vuelo (examen)");

            // Orden: flujo operativo (vuelo → reserva → confirmar → asiento → tiquete) y luego consultas al mapa, por último cambio de asiento.
            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "1. Seleccionar vuelo (contexto)",
                    "2. Crear reserva",
                    "3. Confirmar reserva",
                    "4. Seleccionar asiento en reserva",
                    "5. Asientos asignados a una reserva",
                    "6. Ver tiquete",
                    "7. Ver asientos por vuelo",
                    "8. Consultar disponibilidad por clase",
                    "9. Ver asientos libres de una clase",
                    "10. Ver asientos ocupados",
                    "11. Cambiar asiento",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "1. Seleccionar vuelo (contexto)":
                    await SeleccionarVueloAsync();
                    break;
                case "2. Crear reserva":
                    await CrearReservaAsync();
                    break;
                case "3. Confirmar reserva":
                    await ConfirmarReservaAsync();
                    break;
                case "4. Seleccionar asiento en reserva":
                    await SeleccionarAsientoEnReservaAsync();
                    break;
                case "5. Asientos asignados a una reserva":
                    await AsientosAsignadosAReservaClienteAsync();
                    break;
                case "6. Ver tiquete":
                    await VerTiqueteAsync();
                    break;
                case "7. Ver asientos por vuelo":
                    await VerAsientosPorVueloAsync();
                    break;
                case "8. Consultar disponibilidad por clase":
                    await ConsultarDisponibilidadPorClaseAsync();
                    break;
                case "9. Ver asientos libres de una clase":
                    await VerAsientosLibresDeClaseAsync();
                    break;
                case "10. Ver asientos ocupados":
                    await VerAsientosOcupadosAsync();
                    break;
                case "11. Cambiar asiento":
                    await CambiarAsientoAsync();
                    break;
                case "Volver":
                    return;
            }
        }
    }

    private static async Task<int> ResolverVueloIdConSelectorAsync(IServiceProvider provider)
    {
        SpectreHelper.MostrarSubtitulo("Seleccione el vuelo");
        var vuelo = await SelectorUI.SeleccionarVueloProgramadoAsync(provider)
            ?? throw new InvalidOperationException("Vuelo no seleccionado.");
        return vuelo.Id;
    }

    private Task VerAsientosPorVueloAsync()
    {
        return ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            var vueloId = _vueloIdSeleccionado
                ?? (await ResolverVueloIdConSelectorAsync(_provider));
            if (_vueloIdSeleccionado is null) _vueloIdSeleccionado = vueloId;

            await using var scope = _provider.CreateAsyncScope();
            var asientos = await scope.ServiceProvider
                .GetRequiredService<GetSeatDetailsByFlightUseCase>()
                .ExecuteAsync(vueloId);

            if (asientos.Count == 0)
            {
                SpectreHelper.MostrarInfo("No hay asientos para este vuelo.");
                SpectreHelper.EsperarTecla();
                return;
            }

            foreach (var grupo in asientos
                         .GroupBy(s => new { s.FlightClassId, s.FlightClassName })
                         .OrderBy(g => g.Key.FlightClassName))
            {
                SpectreHelper.MostrarSubtitulo(
                    $"Clase: {grupo.Key.FlightClassName} (id {grupo.Key.FlightClassId})");
                var tabla = SpectreHelper.CrearTabla("N.º asiento", "Estado");
                foreach (var s in grupo.OrderBy(x => x.SeatNumber))
                {
                    SpectreHelper.AgregarFila(tabla,
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

    private Task SeleccionarAsientoEnReservaAsync()
    {
        return ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scopeC = _provider.CreateAsyncScope();
            var clienteId = await ObtenerClienteIdAsync(scopeC.ServiceProvider);
            var reserva = await SelectorUI.SeleccionarReservaAsync(_provider, clienteId);
            if (reserva is null) return;
            await ExamenAsientoReservaUI.EjecutarSeleccionAsientoEnReservaAsync(_provider, reserva);
            SpectreHelper.MostrarExito("Asiento reservado; estado: Reservado (MySQL, transacción atómica).");
            SpectreHelper.EsperarTecla();
        });
    }

    private Task AsientosAsignadosAReservaClienteAsync()
    {
        return ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scopeC = _provider.CreateAsyncScope();
            var clienteId = await ObtenerClienteIdAsync(scopeC.ServiceProvider);
            var reserva = await SelectorUI.SeleccionarReservaAsync(_provider, clienteId);
            if (reserva is null) return;
            await using var scope = _provider.CreateAsyncScope();
            var asientos = await scope.ServiceProvider
                .GetRequiredService<GetSeatsByBookingUseCase>()
                .ExecuteAsync(reserva.Id);
            if (asientos.Count == 0)
            {
                SpectreHelper.MostrarInfo("Esta reserva no tiene asientos asignados en el mapa del vuelo.");
                SpectreHelper.EsperarTecla();
                return;
            }
            var tabla = SpectreHelper.CrearTabla("N.º asiento", "Clase", "Estado");
            foreach (var s in asientos.OrderBy(x => x.SeatNumber))
            {
                SpectreHelper.AgregarFila(tabla,
                    s.SeatNumber,
                    s.FlightClassName,
                    SpectreHelper.FormatearEstadoAsientoExamen(s.Status));
            }
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.MostrarLeyendaEstadosAsientoExamen();
            SpectreHelper.MostrarInfo(
                $"Reserva [{reserva.CodigoReserva.Valor}]  asientos: {asientos.Count}");
            SpectreHelper.EsperarTecla();
        });
    }

    private Task ConsultarDisponibilidadPorClaseAsync()
    {
        return ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            var vueloId = _vueloIdSeleccionado
                ?? (await ResolverVueloIdConSelectorAsync(_provider));
            if (_vueloIdSeleccionado is null) _vueloIdSeleccionado = vueloId;

            await using var scope = _provider.CreateAsyncScope();
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
            {
                SpectreHelper.AgregarFila(tabla,
                    s.FlightClassId.ToString(),
                    s.FlightClassCode,
                    s.FlightClassName,
                    s.Total.ToString(),
                    s.Available.ToString(),
                    s.Reserved.ToString(),
                    s.Occupied.ToString(),
                    s.Blocked.ToString());
            }
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private Task VerAsientosOcupadosAsync()
    {
        return ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            var vueloId = _vueloIdSeleccionado
                ?? (await ResolverVueloIdConSelectorAsync(_provider));
            if (_vueloIdSeleccionado is null) _vueloIdSeleccionado = vueloId;

            await using var scope = _provider.CreateAsyncScope();
            var ocupados = await scope.ServiceProvider
                .GetRequiredService<GetSeatDetailsByFlightUseCase>()
                .ExecuteAsync(vueloId, status: "Occupied");

            if (ocupados.Count == 0)
            {
                SpectreHelper.MostrarInfo("No hay asientos en estado Ocupado en este vuelo.");
                SpectreHelper.EsperarTecla();
                return;
            }

            var tabla = SpectreHelper.CrearTabla("N.º asiento", "Clase", "Estado");
            foreach (var s in ocupados)
            {
                SpectreHelper.AgregarFila(tabla,
                    s.SeatNumber,
                    s.FlightClassName,
                    SpectreHelper.FormatearEstadoAsientoExamen(s.Status));
            }
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.MostrarLeyendaEstadosAsientoExamen();
            SpectreHelper.MostrarInfo($"Total ocupados: {ocupados.Count}");
            SpectreHelper.EsperarTecla();
        });
    }

    private Task CambiarAsientoAsync()
    {
        return ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scopeC = _provider.CreateAsyncScope();
            var clienteId = await ObtenerClienteIdAsync(scopeC.ServiceProvider);
            var reserva = await SelectorUI.SeleccionarReservaAsync(_provider, clienteId);
            if (reserva is null) return;

            await using var scope = _provider.CreateAsyncScope();
            var sp = scope.ServiceProvider;
            var pasajeros = await sp.GetRequiredService<GetPassengersByBookingUseCase>()
                .ExecuteAsync(reserva.Id);
            var conAsiento = pasajeros.Where(p => p.AsientoId.HasValue).ToList();
            if (conAsiento.Count == 0)
            {
                SpectreHelper.MostrarInfo(
                    "Ningún pasajero tiene asiento; use «Seleccionar asiento en reserva» primero.");
                return;
            }

            var pasajero = SpectreHelper.SeleccionarOpcion(
                "Pasajero (debe estar en asiento reservado, sin tiquete emitido aún)",
                conAsiento,
                p => $"  #{p.Id}  persona {p.PersonaId}  asiento: {p.AsientoId}");

            var vueloId = reserva.VueloId;
            _vueloIdSeleccionado = vueloId;

            var clases = await sp.GetRequiredService<GetAvailableFlightClassesByFlightUseCase>()
                .ExecuteAsync(vueloId);
            if (clases.Count == 0)
                throw new InvalidOperationException("No hay clases con asientos libres en ese vuelo.");

            var claseSel = SpectreHelper.SeleccionarOpcion(
                "Nueva clase de vuelo",
                clases,
                c => $"  [{c.FlightClassCode}] {c.FlightClassName}  ({c.Available} libres)");

            var asientos = await sp.GetRequiredService<GetAvailableSeatsByFlightAndClassUseCase>()
                .ExecuteAsync(vueloId, claseSel.FlightClassId);
            if (asientos.Count == 0)
                throw new InvalidOperationException("No hay asientos libres en esa clase.");

            var asientoSel = SpectreHelper.SeleccionarOpcion(
                "Nuevo asiento",
                asientos,
                s => $"  {s.SeatNumber}  —  {s.FlightClassName}");

            _ = await sp.GetRequiredService<ChangeSeatUseCase>()
                .ExecuteAsync(pasajero.Id, claseSel.FlightClassId, asientoSel.SeatNumber);

            SpectreHelper.MostrarExito("Cambio de asiento completado (disponibilidad y registro de asientos).");
            SpectreHelper.EsperarTecla();
        });
    }

    private Task CrearReservaAsync()
    {
        SpectreHelper.MostrarInfo("Búsqueda de vuelo y creación de reserva (flujo base del sistema).");
        SpectreHelper.EsperarTecla();
        return new FlightSearchMenu(_provider, _session).MostrarAsync();
    }

    private Task SeleccionarVueloAsync()
    {
        return ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            SpectreHelper.MostrarSubtitulo("Vuelo para las consultas siguientes");
            var vuelo = await SelectorUI.SeleccionarVueloProgramadoAsync(_provider);
            if (vuelo is null) return;
            _vueloIdSeleccionado = vuelo.Id;
            _flightClassSeleccionadaId = null;
            SpectreHelper.MostrarExito(
                $"Vuelo: {vuelo.NumeroVuelo.Valor} (id {vuelo.Id}) — " +
                "se usará en «Consultar disponibilidad», asientos, etc., si no vuelve a elegir.");
            SpectreHelper.EsperarTecla();
        });
    }

    private Task VerAsientosLibresDeClaseAsync()
    {
        return ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            var vueloId = _vueloIdSeleccionado
                ?? (await ResolverVueloIdConSelectorAsync(_provider));
            if (_vueloIdSeleccionado is null) _vueloIdSeleccionado = vueloId;
            _flightClassSeleccionadaId = null;

            await using var scope = _provider.CreateAsyncScope();
            var clases = await scope.ServiceProvider
                .GetRequiredService<GetAvailableFlightClassesByFlightUseCase>()
                .ExecuteAsync(vueloId);
            if (clases.Count == 0)
            {
                var stats = await scope.ServiceProvider
                    .GetRequiredService<GetSeatStatsByFlightUseCase>()
                    .ExecuteAsync(vueloId);
                if (stats.Count > 0)
                {
                    var tablaStats = SpectreHelper.CrearTabla(
                        "Clase", "Total", "Disp", "Res", "Ocup", "Bloq");
                    foreach (var s in stats)
                    {
                        SpectreHelper.AgregarFila(tablaStats,
                            s.FlightClassName,
                            s.Total.ToString(),
                            s.Available.ToString(),
                            s.Reserved.ToString(),
                            s.Occupied.ToString(),
                            s.Blocked.ToString());
                    }
                    SpectreHelper.MostrarTabla(tablaStats);
                }
                SpectreHelper.MostrarInfo("No hay asientos en estado libre (Disponible) para vender o elegir.");
                SpectreHelper.EsperarTecla();
                return;
            }

            var flightClassId = _flightClassSeleccionadaId;
            if (!flightClassId.HasValue)
            {
                var sel = SpectreHelper.SeleccionarOpcion(
                    "Clase",
                    clases,
                    c => $"  [{c.FlightClassCode}] {c.FlightClassName}  ({c.Available} libres)");
                flightClassId = sel.FlightClassId;
            }

            var asientos = await scope.ServiceProvider
                .GetRequiredService<GetAvailableSeatsByFlightAndClassUseCase>()
                .ExecuteAsync(vueloId, flightClassId.Value);

            var tabla = SpectreHelper.CrearTabla("N.º asiento", "Clase", "Estado");
            foreach (var s in asientos)
            {
                SpectreHelper.AgregarFila(tabla,
                    s.SeatNumber,
                    s.FlightClassName,
                    SpectreHelper.FormatearEstadoAsientoExamen(s.Status));
            }
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.MostrarInfo($"Libres en esa clase: {asientos.Count}");
            SpectreHelper.EsperarTecla();
        });
    }

    private Task ConfirmarReservaAsync()
    {
        return ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scopeC = _provider.CreateAsyncScope();
            var clienteId = await ObtenerClienteIdAsync(scopeC.ServiceProvider);
            var reserva = await SelectorUI.SeleccionarReservaAsync(_provider, clienteId);
            if (reserva is null) return;

            await using var scope = _provider.CreateAsyncScope();
            _ = await scope.ServiceProvider.GetRequiredService<ConfirmBookingUseCase>()
                .ExecuteAsync(
                    reserva.Id,
                    _session.CurrentUserId > 0 ? _session.CurrentUserId : null);

            SpectreHelper.MostrarExito("Reserva confirmada.");
            SpectreHelper.EsperarTecla();
        });
    }

    private Task VerTiqueteAsync()
    {
        SpectreHelper.MostrarInfo("Tiquetes y pase de abordar: módulo «Mis reservas».");
        SpectreHelper.EsperarTecla();
        return new MyBookingsMenu(_provider, _session).MostrarAsync();
    }

    private async Task<int> ObtenerClienteIdAsync(IServiceProvider sp)
    {
        var clientes = await sp.GetRequiredService<GetAllClientsUseCase>()
            .ExecuteAsync();
        return clientes.FirstOrDefault(c => c.UsuarioId == _session.CurrentUserId)?.Id
            ?? throw new InvalidOperationException(
                "No se encontró el perfil de cliente. Use un usuario con rol Cliente.");
    }
}
