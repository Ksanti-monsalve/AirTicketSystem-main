// src/UI/Admin/Reportes/ReportesMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.flight.Application.UseCases;
using AirTicketSystem.modules.seatavailability.Application.UseCases;
using AirTicketSystem.modules.client.Application.UseCases;
using AirTicketSystem.modules.booking.Application.UseCases;
using AirTicketSystem.modules.route.Application.UseCases;
using AirTicketSystem.modules.airline.Application.UseCases;
using AirTicketSystem.modules.flighthistory.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Reportes;

public sealed class ReportesMenu
{
    private readonly IServiceProvider _provider;

    public ReportesMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Reportes LINQ");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione un reporte",
                [
                    "8.1 Vuelos con mayor ocupación",
                    "8.2 Vuelos con asientos disponibles",
                    "8.3 Clientes con más reservas",
                    "8.4 Destinos más solicitados",
                    "8.5 Reservas por estado",
                    "8.6 Ingresos estimados por aerolínea",
                    "8.7 Historial de cambios por rango de fechas",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "8.1 Vuelos con mayor ocupación":        await Reporte1Async(); break;
                case "8.2 Vuelos con asientos disponibles":   await Reporte2Async(); break;
                case "8.3 Clientes con más reservas":         await Reporte3Async(); break;
                case "8.4 Destinos más solicitados":          await Reporte4Async(); break;
                case "8.5 Reservas por estado":               await Reporte5Async(); break;
                case "8.6 Ingresos estimados por aerolínea":  await Reporte6Async(); break;
                case "8.7 Historial de cambios por rango de fechas": await Reporte7Async(); break;
                case "Volver": return;
            }
        }
    }

    // ── 8.1 Vuelos con mayor ocupación ───────────────────────────────────────
    private async Task Reporte1Async()
    {
        SpectreHelper.MostrarSubtitulo("8.1 — Vuelos con Mayor Ocupación");
        var top = SpectreHelper.PedirEntero("Mostrar top N vuelos (ej: 10)");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var vuelos     = await scope.ServiceProvider.GetRequiredService<GetAllFlightsUseCase>().ExecuteAsync();
            var availUc    = scope.ServiceProvider.GetRequiredService<GetAvailableSeatsByFlightUseCase>();

            // LINQ: para cada vuelo obtener asientos disponibles y calcular ocupación
            var resultado = new List<(int vueloId, string numero, string estado, int disponibles)>();
            foreach (var v in vuelos)
            {
                var disponibles = await availUc.ExecuteAsync(v.Id);
                resultado.Add((v.Id, v.NumeroVuelo.Valor, v.Estado.Valor, disponibles.Count));
            }

            var reporte = resultado
                .Where(r => r.estado != "CANCELADO")
                .OrderBy(r => r.disponibles)   // menos disponibles = más ocupado
                .Take(top)
                .ToList();

            if (reporte.Count == 0) { SpectreHelper.MostrarInfo("Sin datos de vuelos."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("VueloID", "Número", "Estado", "Asientos disponibles");
            foreach (var (vueloId, numero, estado, disponibles) in reporte)
                SpectreHelper.AgregarFila(tabla, vueloId.ToString(), numero, estado, disponibles.ToString());
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    // ── 8.2 Vuelos con asientos disponibles ──────────────────────────────────
    private async Task Reporte2Async()
    {
        SpectreHelper.MostrarSubtitulo("8.2 — Vuelos con Asientos Disponibles");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var vuelos  = await scope.ServiceProvider.GetRequiredService<GetScheduledFlightsUseCase>().ExecuteAsync();
            var availUc = scope.ServiceProvider.GetRequiredService<GetAvailableSeatsByFlightUseCase>();

            var resultado = new List<(int id, string numero, string ruta, int disponibles)>();
            foreach (var v in vuelos)
            {
                var disponibles = await availUc.ExecuteAsync(v.Id);
                if (disponibles.Count > 0)
                    resultado.Add((v.Id, v.NumeroVuelo.Valor, $"Ruta #{v.RutaId}", disponibles.Count));
            }

            // LINQ: ordenar por más asientos disponibles
            var reporte = resultado
                .OrderByDescending(r => r.disponibles)
                .ToList();

            if (reporte.Count == 0) { SpectreHelper.MostrarInfo("Sin vuelos con asientos disponibles."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("VueloID", "Número", "Ruta", "Asientos disponibles");
            foreach (var (id, numero, ruta, disponibles) in reporte)
                SpectreHelper.AgregarFila(tabla, id.ToString(), numero, ruta, disponibles.ToString());
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    // ── 8.3 Clientes con más reservas ────────────────────────────────────────
    private async Task Reporte3Async()
    {
        SpectreHelper.MostrarSubtitulo("8.3 — Clientes con Más Reservas");
        var top = SpectreHelper.PedirEntero("Mostrar top N clientes (ej: 10)");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var clientes   = await scope.ServiceProvider.GetRequiredService<GetAllClientsUseCase>().ExecuteAsync();
            var bookingUc  = scope.ServiceProvider.GetRequiredService<GetBookingsByClienteUseCase>();

            var datos = new List<(int clienteId, int personaId, int reservas)>();
            foreach (var c in clientes)
            {
                var reservas = await bookingUc.ExecuteAsync(c.Id);
                datos.Add((c.Id, c.PersonaId, reservas.Count));
            }

            // LINQ: ordenar por cantidad de reservas descendente
            var reporte = datos
                .OrderByDescending(d => d.reservas)
                .Take(top)
                .ToList();

            if (reporte.Count == 0) { SpectreHelper.MostrarInfo("Sin datos de clientes."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ClienteID", "PersonaID", "Total reservas");
            foreach (var (clienteId, personaId, reservas) in reporte)
                SpectreHelper.AgregarFila(tabla, clienteId.ToString(), personaId.ToString(), reservas.ToString());
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    // ── 8.4 Destinos más solicitados ─────────────────────────────────────────
    private async Task Reporte4Async()
    {
        SpectreHelper.MostrarSubtitulo("8.4 — Destinos Más Solicitados");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope  = _provider.CreateAsyncScope();
            var rutas    = await scope.ServiceProvider.GetRequiredService<GetAllRoutesUseCase>().ExecuteAsync();
            var vueloUc  = scope.ServiceProvider.GetRequiredService<GetFlightsByRouteUseCase>();

            var conteos = new List<(int aeropuertoDestino, int rutaId, int vuelos)>();
            foreach (var r in rutas)
            {
                var vuelos = await vueloUc.ExecuteAsync(r.Id);
                conteos.Add((r.DestinoId, r.Id, vuelos.Count));
            }

            // LINQ: agrupar por aeropuerto destino, sumar vuelos, ordenar descendente
            var reporte = conteos
                .GroupBy(c => c.aeropuertoDestino)
                .Select(g => new
                {
                    AeropuertoDestinoID = g.Key,
                    TotalRutas          = g.Count(),
                    TotalVuelos         = g.Sum(x => x.vuelos)
                })
                .OrderByDescending(x => x.TotalVuelos)
                .ToList();

            // DestinoId es la propiedad correcta del aggregate Route

            if (reporte.Count == 0) { SpectreHelper.MostrarInfo("Sin datos de rutas."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("AeropuertoDestinoID", "Total rutas", "Total vuelos");
            foreach (var r in reporte)
                SpectreHelper.AgregarFila(tabla, r.AeropuertoDestinoID.ToString(), r.TotalRutas.ToString(), r.TotalVuelos.ToString());
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    // ── 8.5 Reservas por estado ───────────────────────────────────────────────
    private async Task Reporte5Async()
    {
        SpectreHelper.MostrarSubtitulo("8.5 — Reservas por Estado");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var clientes  = await scope.ServiceProvider.GetRequiredService<GetAllClientsUseCase>().ExecuteAsync();
            var bookingUc = scope.ServiceProvider.GetRequiredService<GetBookingsByClienteUseCase>();

            var todasReservas = new List<string>();
            foreach (var c in clientes)
            {
                var reservas = await bookingUc.ExecuteAsync(c.Id);
                todasReservas.AddRange(reservas.Select(r => r.Estado.Valor));
            }

            // LINQ: agrupar por estado y contar
            var reporte = todasReservas
                .GroupBy(estado => estado)
                .Select(g => new { Estado = g.Key, Total = g.Count() })
                .OrderByDescending(x => x.Total)
                .ToList();

            if (reporte.Count == 0) { SpectreHelper.MostrarInfo("Sin reservas registradas."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("Estado", "Total reservas");
            foreach (var r in reporte)
                SpectreHelper.AgregarFila(tabla, r.Estado, r.Total.ToString());
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    // ── 8.6 Ingresos estimados por aerolínea ─────────────────────────────────
    private async Task Reporte6Async()
    {
        SpectreHelper.MostrarSubtitulo("8.6 — Ingresos Estimados por Aerolínea");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope    = _provider.CreateAsyncScope();
            var aerolineas  = await scope.ServiceProvider.GetRequiredService<GetAllAirlinesUseCase>().ExecuteAsync();
            var rutasUc     = scope.ServiceProvider.GetRequiredService<GetRoutesByAirlineUseCase>();
            var vuelosUc    = scope.ServiceProvider.GetRequiredService<GetFlightsByRouteUseCase>();
            var clientesUc  = scope.ServiceProvider.GetRequiredService<GetAllClientsUseCase>();
            var bookingUc   = scope.ServiceProvider.GetRequiredService<GetBookingsByClienteUseCase>();

            // Obtener todas las reservas confirmadas
            var clientes = await clientesUc.ExecuteAsync();
            var reservasConVuelo = new List<(int vueloId, decimal total)>();
            foreach (var c in clientes)
            {
                var reservas = await bookingUc.ExecuteAsync(c.Id);
                foreach (var r in reservas.Where(r => r.Estado.Valor == "CONFIRMADA"))
                    reservasConVuelo.Add((r.VueloId, r.ValorTotal.Valor));
            }

            var resultado = new List<(int aerolineaId, string nombre, decimal ingresos, int vuelos)>();
            foreach (var a in aerolineas)
            {
                var rutas = await rutasUc.ExecuteAsync(a.Id);
                var vueloIds = new List<int>();
                foreach (var r in rutas)
                {
                    var vuelos = await vuelosUc.ExecuteAsync(r.Id);
                    vueloIds.AddRange(vuelos.Select(v => v.Id));
                }

                // LINQ: suma de ingresos de reservas confirmadas de vuelos de esta aerolínea
                var ingresos = reservasConVuelo
                    .Where(rv => vueloIds.Contains(rv.vueloId))
                    .Sum(rv => rv.total);

                resultado.Add((a.Id, a.Nombre.Valor, ingresos, vueloIds.Count));
            }

            var reporte = resultado
                .OrderByDescending(r => r.ingresos)
                .ToList();

            if (reporte.Count == 0) { SpectreHelper.MostrarInfo("Sin datos de aerolíneas."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("AerolineaID", "Nombre", "Total vuelos", "Ingresos estimados");
            foreach (var r in reporte)
                SpectreHelper.AgregarFila(tabla, r.aerolineaId.ToString(), r.nombre, r.vuelos.ToString(), r.ingresos.ToString("C"));
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    // ── 8.7 Historial de cambios de vuelo por rango de fechas ────────────────
    private async Task Reporte7Async()
    {
        SpectreHelper.MostrarSubtitulo("8.7 — Historial de Cambios por Rango de Fechas");
        var desdeStr = SpectreHelper.PedirTexto("Fecha desde (yyyy-MM-dd)");
        var hastaStr = SpectreHelper.PedirTexto("Fecha hasta (yyyy-MM-dd)");

        if (!DateTime.TryParse(desdeStr, out var desde) || !DateTime.TryParse(hastaStr, out var hasta))
        {
            SpectreHelper.MostrarError("Fechas inválidas."); SpectreHelper.EsperarTecla(); return;
        }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope  = _provider.CreateAsyncScope();
            var vuelos    = await scope.ServiceProvider.GetRequiredService<GetAllFlightsUseCase>().ExecuteAsync();
            var histUc    = scope.ServiceProvider.GetRequiredService<GetFlightHistoryUseCase>();

            var todosLogs = new List<(int vueloId, string numero, string anterior, string nuevo, DateTime fecha, string? motivo)>();
            foreach (var v in vuelos)
            {
                var historial = await histUc.ExecuteAsync(v.Id);
                foreach (var h in historial)
                    todosLogs.Add((v.Id, v.NumeroVuelo.Valor, h.EstadoAnterior.Valor, h.EstadoNuevo.Valor, h.FechaCambio, h.Motivo?.Valor));
            }

            // LINQ: filtrar por rango de fechas y ordenar por fecha
            var reporte = todosLogs
                .Where(l => l.fecha >= desde && l.fecha <= hasta.AddDays(1))
                .OrderByDescending(l => l.fecha)
                .ToList();

            if (reporte.Count == 0) { SpectreHelper.MostrarInfo("Sin cambios en el rango indicado."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("VueloID", "Número", "Antes", "Después", "Fecha", "Motivo");
            foreach (var (vueloId, numero, anterior, nuevo, fecha, motivo) in reporte)
                SpectreHelper.AgregarFila(tabla,
                    vueloId.ToString(), numero, anterior, nuevo,
                    fecha.ToString("yyyy-MM-dd HH:mm"),
                    motivo ?? "-");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }
}
