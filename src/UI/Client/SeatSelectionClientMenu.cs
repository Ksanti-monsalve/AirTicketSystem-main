using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.seat.Application.UseCases;
using AirTicketSystem.modules.booking.Application.UseCases;
using AirTicketSystem.modules.bookingpassenger.Application.UseCases;
using AirTicketSystem.modules.client.Application.UseCases;

namespace AirTicketSystem.UI.Client;

/// <summary>
/// Menú solicitado por el examen para separar funciones del CLIENTE
/// relacionadas con selección de vuelo/clase/asiento, confirmación y consulta de tiquete.
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
            SpectreHelper.MostrarTitulo("CLIENTE — Selección de Asientos y Clases de Vuelo");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "1. Crear reserva",
                    "2. Seleccionar vuelo",
                    "3. Ver clases disponibles",
                    "4. Ver asientos disponibles",
                    "5. Seleccionar asiento",
                    "6. Confirmar reserva",
                    "7. Ver tiquete",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "1. Crear reserva":
                    await CrearReservaAsync();
                    break;
                case "2. Seleccionar vuelo":
                    await SeleccionarVueloAsync();
                    break;
                case "3. Ver clases disponibles":
                    await VerClasesDisponiblesAsync();
                    break;
                case "4. Ver asientos disponibles":
                    await VerAsientosDisponiblesAsync();
                    break;
                case "5. Seleccionar asiento":
                    await SeleccionarAsientoAsync();
                    break;
                case "6. Confirmar reserva":
                    await ConfirmarReservaAsync();
                    break;
                case "7. Ver tiquete":
                    await VerTiqueteAsync();
                    break;
                case "Volver":
                    return;
            }
        }
    }

    private Task CrearReservaAsync()
    {
        SpectreHelper.MostrarInfo("Se abrirá el módulo actual de búsqueda de vuelos y creación de reserva.");
        SpectreHelper.EsperarTecla();
        return new FlightSearchMenu(_provider, _session).MostrarAsync();
    }

    private Task SeleccionarVueloAsync()
    {
        return ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            SpectreHelper.MostrarSubtitulo("Seleccione el vuelo");
            var vuelo = await SelectorUI.SeleccionarVueloProgramadoAsync(_provider);
            if (vuelo is null) return;
            _vueloIdSeleccionado = vuelo.Id;
            _flightClassSeleccionadaId = null;
            SpectreHelper.MostrarExito($"Vuelo seleccionado: {vuelo.NumeroVuelo.Valor} (ID {vuelo.Id}).");
            SpectreHelper.EsperarTecla();
        });
    }

    private Task VerClasesDisponiblesAsync()
    {
        return ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            var vueloId = _vueloIdSeleccionado;
            if (!vueloId.HasValue)
            {
                SpectreHelper.MostrarSubtitulo("Seleccione el vuelo");
                var vuelo = await SelectorUI.SeleccionarVueloProgramadoAsync(_provider);
                if (vuelo is null) return;
                vueloId = vuelo.Id;
                _vueloIdSeleccionado = vueloId;
                _flightClassSeleccionadaId = null;
            }

            await using var scope = _provider.CreateAsyncScope();
            var clases = await scope.ServiceProvider
                .GetRequiredService<GetAvailableFlightClassesByFlightUseCase>()
                .ExecuteAsync(vueloId.Value);

            if (clases.Count == 0)
            {
                // Diagnóstico útil: si hay asientos pero ninguno DISPONIBLE, mostrar stats.
                var stats = await scope.ServiceProvider
                    .GetRequiredService<GetSeatStatsByFlightUseCase>()
                    .ExecuteAsync(vueloId.Value);
                if (stats.Count > 0)
                {
                    var tablaStats = SpectreHelper.CrearTabla("Clase", "Total", "Disp", "Res", "Ocup", "Bloq");
                    foreach (var s in stats)
                        SpectreHelper.AgregarFila(tablaStats,
                            s.FlightClassName,
                            s.Total.ToString(),
                            s.Available.ToString(),
                            s.Reserved.ToString(),
                            s.Occupied.ToString(),
                            s.Blocked.ToString());
                    SpectreHelper.MostrarTabla(tablaStats);
                }

                SpectreHelper.MostrarInfo("No hay clases con asientos DISPONIBLES para ese vuelo.");
                SpectreHelper.EsperarTecla();
                return;
            }

            var tabla = SpectreHelper.CrearTabla("FlightClassId", "Code", "Name", "Available");
            foreach (var c in clases)
                SpectreHelper.AgregarFila(tabla,
                    c.FlightClassId.ToString(),
                    c.FlightClassCode,
                    c.FlightClassName,
                    c.Available.ToString());
            SpectreHelper.MostrarTabla(tabla);

            var seleccion = SpectreHelper.SeleccionarOpcion(
                "Seleccione la clase",
                clases,
                c => $"  [{c.FlightClassCode}] {c.FlightClassName}  ({c.Available} disponibles)");

            _vueloIdSeleccionado = vueloId.Value;
            _flightClassSeleccionadaId = seleccion.FlightClassId;
            SpectreHelper.MostrarExito($"Clase seleccionada: {seleccion.FlightClassName}.");
            SpectreHelper.EsperarTecla();
        });
    }

    private Task VerAsientosDisponiblesAsync()
    {
        return ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            SpectreHelper.MostrarInfo("Consultando asientos disponibles...");
            var vueloId = _vueloIdSeleccionado;
            if (!vueloId.HasValue)
            {
                SpectreHelper.MostrarSubtitulo("Seleccione el vuelo");
                var vuelo = await SelectorUI.SeleccionarVueloProgramadoAsync(_provider);
                if (vuelo is null) return;
                vueloId = vuelo.Id;
                _vueloIdSeleccionado = vueloId;
                _flightClassSeleccionadaId = null;
            }

            var flightClassId = _flightClassSeleccionadaId;

            await using var scope = _provider.CreateAsyncScope();
            if (!flightClassId.HasValue)
            {
                var clases = await scope.ServiceProvider
                    .GetRequiredService<GetAvailableFlightClassesByFlightUseCase>()
                    .ExecuteAsync(vueloId.Value);
                if (clases.Count == 0)
                {
                    var stats = await scope.ServiceProvider
                        .GetRequiredService<GetSeatStatsByFlightUseCase>()
                        .ExecuteAsync(vueloId.Value);
                    if (stats.Count > 0)
                    {
                        var tablaStats = SpectreHelper.CrearTabla("Clase", "Total", "Disp", "Res", "Ocup", "Bloq");
                        foreach (var s in stats)
                            SpectreHelper.AgregarFila(tablaStats,
                                s.FlightClassName,
                                s.Total.ToString(),
                                s.Available.ToString(),
                                s.Reserved.ToString(),
                                s.Occupied.ToString(),
                                s.Blocked.ToString());
                        SpectreHelper.MostrarTabla(tablaStats);
                    }
                    SpectreHelper.MostrarInfo("No hay asientos disponibles para ese vuelo.");
                    SpectreHelper.EsperarTecla();
                    return;
                }
                var sel = SpectreHelper.SeleccionarOpcion(
                    "Seleccione la clase",
                    clases,
                    c => $"  [{c.FlightClassCode}] {c.FlightClassName}  ({c.Available} disponibles)");
                flightClassId = sel.FlightClassId;
                _vueloIdSeleccionado = vueloId.Value;
                _flightClassSeleccionadaId = flightClassId;
            }

            var asientos = await scope.ServiceProvider
                .GetRequiredService<GetAvailableSeatsByFlightAndClassUseCase>()
                .ExecuteAsync(vueloId.Value, flightClassId.Value);

            SpectreHelper.MostrarInfo($"FlightId={vueloId.Value} FlightClassId={flightClassId.Value} → Available={asientos.Count}");

            if (asientos.Count == 0)
            {
                SpectreHelper.MostrarInfo("No hay asientos DISPONIBLES para ese vuelo.");
                SpectreHelper.EsperarTecla();
                return;
            }

            // Regla examen: cliente SOLO ve DISPONIBLES
            var tabla = SpectreHelper.CrearTabla("N° asiento", "Clase", "Estado");
            foreach (var s in asientos)
                SpectreHelper.AgregarFila(tabla,
                    s.SeatNumber,
                    s.FlightClassName,
                    s.Status);
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.MostrarInfo($"Total disponibles: {asientos.Count}");
            SpectreHelper.EsperarTecla();
        });
    }

    private Task SeleccionarAsientoAsync()
    {
        return ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scopeC = _provider.CreateAsyncScope();
            var clienteId = await ObtenerClienteIdAsync(scopeC.ServiceProvider);

            // 1) Seleccionar reserva del cliente
            var reserva = await SelectorUI.SeleccionarReservaAsync(_provider, clienteId);
            if (reserva is null) return;

            var vueloId = reserva.VueloId;

            // 2) Seleccionar pasajero de la reserva
            await using var scope = _provider.CreateAsyncScope();
            var pasajeros = await scope.ServiceProvider
                .GetRequiredService<GetPassengersByBookingUseCase>()
                .ExecuteAsync(reserva.Id);
            if (pasajeros.Count == 0)
                throw new InvalidOperationException("La reserva no tiene pasajeros.");

            var pasajero = SpectreHelper.SeleccionarOpcion(
                "Seleccione el pasajero",
                pasajeros,
                p => $"  PasajeroReserva #{p.Id}  PersonaID:{p.PersonaId}  Tipo:{p.TipoPasajero.Valor}  Asiento:{(p.AsientoId?.ToString() ?? "Sin asignar")}");

            // 3) Seleccionar clase (solo clases con asientos disponibles)
            var clases = await scope.ServiceProvider
                .GetRequiredService<GetAvailableFlightClassesByFlightUseCase>()
                .ExecuteAsync(vueloId);
            if (clases.Count == 0)
                throw new InvalidOperationException("No hay clases con asientos disponibles para este vuelo.");

            var claseSel = SpectreHelper.SeleccionarOpcion(
                "Seleccione la clase",
                clases,
                c => $"  [{c.FlightClassCode}] {c.FlightClassName}  ({c.Available} disponibles)");

            // 4) Mostrar asientos disponibles de esa clase y elegir número (ej: 10A)
            var asientos = await scope.ServiceProvider
                .GetRequiredService<GetAvailableSeatsByFlightAndClassUseCase>()
                .ExecuteAsync(vueloId, claseSel.FlightClassId);
            if (asientos.Count == 0)
                throw new InvalidOperationException("No hay asientos disponibles para esa clase.");

            var asientoSel = SpectreHelper.SeleccionarOpcion(
                "Seleccione el asiento",
                asientos,
                s => $"  {s.SeatNumber}  —  {s.FlightClassName}");

            // 5) Validaciones + asignación + persistencia (RESERVADO + asociado a pasajero)
            await scope.ServiceProvider
                .GetRequiredService<SelectSeatForPassengerUseCase>()
                .SelectAsync(
                    pasajeroReservaId: pasajero.Id,
                    vueloId: vueloId,
                    flightClassId: claseSel.FlightClassId,
                    seatNumber: asientoSel.SeatNumber);

            SpectreHelper.MostrarExito("Asiento reservado correctamente");
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
                .ExecuteAsync(reserva.Id, _session.CurrentUserId > 0 ? _session.CurrentUserId : null);

            SpectreHelper.MostrarExito("Reserva confirmada.");
            SpectreHelper.EsperarTecla();
        });
    }

    private Task VerTiqueteAsync()
    {
        SpectreHelper.MostrarInfo("Se abrirá el módulo actual de 'Mis reservas' para ver/emitir/consultar tiquetes.");
        SpectreHelper.EsperarTecla();
        return new MyBookingsMenu(_provider, _session).MostrarAsync();
    }

    private async Task<int> ObtenerClienteIdAsync(IServiceProvider sp)
    {
        var clientes = await sp.GetRequiredService<GetAllClientsUseCase>()
            .ExecuteAsync();
        return clientes.FirstOrDefault(c => c.UsuarioId == _session.CurrentUserId)?.Id
            ?? throw new InvalidOperationException(
                "No se encontró el perfil de cliente asociado a su usuario. Contacte al administrador.");
    }
}

