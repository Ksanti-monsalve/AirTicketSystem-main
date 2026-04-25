// src/UI/Admin/Flights/FlightMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.flight.Application.UseCases;
using AirTicketSystem.modules.route.Application.UseCases;
using AirTicketSystem.modules.aircraft.Application.UseCases;
using AirTicketSystem.modules.airport.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Flights;

public sealed class FlightMenu
{
    private readonly IServiceProvider _provider;

    public FlightMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Vuelos");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Listar todos",
                    "Listar programados",
                    "Listar por fecha",
                    "Listar por ruta",
                    "Listar con check-in abierto",
                    "Crear vuelo",
                    "Editar horarios / puerta",
                    "Asignar puerta",
                    "Abrir check-in",
                    "Iniciar abordaje",
                    "Iniciar vuelo",
                    "Registrar aterrizaje",
                    "Demorar vuelo",
                    "Cancelar vuelo",
                    "Desviar vuelo",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Listar todos":              await ListarTodosAsync();           break;
                case "Listar programados":        await ListarProgramadosAsync();     break;
                case "Listar por fecha":          await ListarPorFechaAsync();        break;
                case "Listar por ruta":           await ListarPorRutaAsync();         break;
                case "Listar con check-in abierto": await ListarCheckinAbiertoAsync(); break;
                case "Crear vuelo":               await CrearAsync();                 break;
                case "Editar horarios / puerta":  await EditarAsync();                break;
                case "Asignar puerta":            await AsignarPuertaAsync();         break;
                case "Abrir check-in":            await AbrirCheckinAsync();          break;
                case "Iniciar abordaje":          await IniciarAbordajeAsync();       break;
                case "Iniciar vuelo":             await IniciarVueloAsync();          break;
                case "Registrar aterrizaje":      await RegistrarAterrizajeAsync();   break;
                case "Demorar vuelo":             await DemorarAsync();               break;
                case "Cancelar vuelo":            await CancelarAsync();              break;
                case "Desviar vuelo":             await DesviarAsync();               break;
                case "Volver":                    return;
            }
        }
    }

    // ── Listados ─────────────────────────────────────────────────────────────

    private async Task ListarTodosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllFlightsUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin vuelos registrados."); SpectreHelper.EsperarTecla(); return; }
            MostrarTablaVuelos(lista);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarProgramadosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetScheduledFlightsUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("No hay vuelos programados."); SpectreHelper.EsperarTecla(); return; }
            MostrarTablaVuelos(lista);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarPorFechaAsync()
    {
        var fechaStr = SpectreHelper.PedirTexto("Fecha (yyyy-MM-dd)");
        if (!DateTime.TryParse(fechaStr, out var fecha))
        {
            SpectreHelper.MostrarError("Fecha inválida."); SpectreHelper.EsperarTecla(); return;
        }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetFlightsByDateUseCase>().ExecuteAsync(fecha);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin vuelos para esa fecha."); SpectreHelper.EsperarTecla(); return; }
            MostrarTablaVuelos(lista);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarPorRutaAsync()
    {
        await MostrarRutasActivasAsync();
        var rutaId = SpectreHelper.PedirEntero("ID de la ruta");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetFlightsByRouteUseCase>().ExecuteAsync(rutaId);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin vuelos para esa ruta."); SpectreHelper.EsperarTecla(); return; }
            MostrarTablaVuelos(lista);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarCheckinAbiertoAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetFlightsWithOpenCheckinUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("No hay vuelos con check-in abierto."); SpectreHelper.EsperarTecla(); return; }
            MostrarTablaVuelos(lista);
            SpectreHelper.EsperarTecla();
        });
    }

    // ── Crear ─────────────────────────────────────────────────────────────────

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Crear Vuelo");

        await MostrarRutasActivasAsync();
        var rutaId = SpectreHelper.PedirEntero("ID de la ruta");

        await MostrarAvionesDisponiblesAsync();
        var avionId = SpectreHelper.PedirEntero("ID del avión");

        var numero      = SpectreHelper.PedirTexto("Número de vuelo (ej: AV100)");
        var salidaStr   = SpectreHelper.PedirTexto("Fecha y hora de salida (yyyy-MM-dd HH:mm)");
        var llegadaStr  = SpectreHelper.PedirTexto("Fecha y hora de llegada estimada (yyyy-MM-dd HH:mm)");

        if (!DateTime.TryParse(salidaStr, out var salida) || !DateTime.TryParse(llegadaStr, out var llegada))
        {
            SpectreHelper.MostrarError("Formato de fecha inválido."); SpectreHelper.EsperarTecla(); return;
        }

        var puertaStr = SpectreHelper.PedirTexto("ID de la puerta de embarque (opcional, Enter para omitir)");
        int? puertaId = int.TryParse(puertaStr, out var pv) && pv > 0 ? pv : null;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var f = await scope.ServiceProvider.GetRequiredService<CreateFlightUseCase>()
                .ExecuteAsync(rutaId, avionId, numero, salida, llegada, puertaId);
            SpectreHelper.MostrarExito($"Vuelo '{f.NumeroVuelo.Valor}' creado (ID {f.Id}). Estado: {f.Estado.Valor}.");
        });
        SpectreHelper.EsperarTecla();
    }

    // ── Editar ────────────────────────────────────────────────────────────────

    private async Task EditarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Editar Horarios / Puerta");
        var id = SpectreHelper.PedirEntero("ID del vuelo");

        var salidaStr  = SpectreHelper.PedirTexto("Nueva fecha/hora de salida (yyyy-MM-dd HH:mm)");
        var llegadaStr = SpectreHelper.PedirTexto("Nueva fecha/hora de llegada estimada (yyyy-MM-dd HH:mm)");

        if (!DateTime.TryParse(salidaStr, out var salida) || !DateTime.TryParse(llegadaStr, out var llegada))
        {
            SpectreHelper.MostrarError("Formato de fecha inválido."); SpectreHelper.EsperarTecla(); return;
        }

        var puertaStr = SpectreHelper.PedirTexto("ID de la puerta de embarque (opcional, Enter para omitir)");
        int? puertaId = int.TryParse(puertaStr, out var pv) && pv > 0 ? pv : null;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var f = await scope.ServiceProvider.GetRequiredService<UpdateFlightUseCase>()
                .ExecuteAsync(id, salida, llegada, puertaId);
            SpectreHelper.MostrarExito($"Vuelo '{f.NumeroVuelo.Valor}' actualizado.");
        });
        SpectreHelper.EsperarTecla();
    }

    // ── Transiciones de estado ────────────────────────────────────────────────

    private async Task AsignarPuertaAsync()
    {
        var vueloId  = SpectreHelper.PedirEntero("ID del vuelo");
        var puertaId = SpectreHelper.PedirEntero("ID de la puerta de embarque");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<AssignGateToFlightUseCase>()
                .ExecuteAsync(vueloId, puertaId);
            SpectreHelper.MostrarExito("Puerta asignada al vuelo.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task AbrirCheckinAsync()
    {
        SpectreHelper.MostrarSubtitulo("Abrir Check-in");
        var id = SpectreHelper.PedirEntero("ID del vuelo");

        var aperturaStr = SpectreHelper.PedirTexto("Fecha/hora de apertura (yyyy-MM-dd HH:mm)");
        var cierreStr   = SpectreHelper.PedirTexto("Fecha/hora de cierre (yyyy-MM-dd HH:mm)");

        if (!DateTime.TryParse(aperturaStr, out var apertura) || !DateTime.TryParse(cierreStr, out var cierre))
        {
            SpectreHelper.MostrarError("Formato de fecha inválido."); SpectreHelper.EsperarTecla(); return;
        }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<OpenCheckinUseCase>()
                .ExecuteAsync(id, apertura, cierre);
            SpectreHelper.MostrarExito("Check-in abierto.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task IniciarAbordajeAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del vuelo");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<StartBoardingUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Abordaje iniciado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task IniciarVueloAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del vuelo");
        if (!SpectreHelper.Confirmar("¿Confirma iniciar el vuelo?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<StartFlightUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Vuelo iniciado. Estado: EN_VUELO.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task RegistrarAterrizajeAsync()
    {
        SpectreHelper.MostrarSubtitulo("Registrar Aterrizaje");
        var id          = SpectreHelper.PedirEntero("ID del vuelo");
        var llegadaStr  = SpectreHelper.PedirTexto("Fecha/hora real de llegada (yyyy-MM-dd HH:mm)");

        if (!DateTime.TryParse(llegadaStr, out var llegada))
        {
            SpectreHelper.MostrarError("Formato de fecha inválido."); SpectreHelper.EsperarTecla(); return;
        }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<RegisterLandingFlightUseCase>()
                .ExecuteAsync(id, llegada);
            SpectreHelper.MostrarExito("Aterrizaje registrado. Estado: ATERRIZADO.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task DemorarAsync()
    {
        var id     = SpectreHelper.PedirEntero("ID del vuelo");
        var motivo = SpectreHelper.PedirTexto("Motivo de la demora");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DelayFlightUseCase>()
                .ExecuteAsync(id, motivo);
            SpectreHelper.MostrarExito("Vuelo marcado como DEMORADO.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task CancelarAsync()
    {
        var id     = SpectreHelper.PedirEntero("ID del vuelo");
        var motivo = SpectreHelper.PedirTexto("Motivo de cancelación");
        if (!SpectreHelper.Confirmar("¿Confirma cancelar el vuelo?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<CancelFlightUseCase>()
                .ExecuteAsync(id, motivo);
            SpectreHelper.MostrarExito("Vuelo cancelado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task DesviarAsync()
    {
        var id     = SpectreHelper.PedirEntero("ID del vuelo");
        var motivo = SpectreHelper.PedirTexto("Motivo del desvío");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DivertFlightUseCase>()
                .ExecuteAsync(id, motivo);
            SpectreHelper.MostrarExito("Vuelo desviado. Estado: DESVIADO.");
        });
        SpectreHelper.EsperarTecla();
    }

    // ── Helpers de visualización ──────────────────────────────────────────────

    private static void MostrarTablaVuelos(
        IEnumerable<AirTicketSystem.modules.flight.Domain.aggregate.Flight> lista)
    {
        var tabla = SpectreHelper.CrearTabla("ID", "Número", "RutaID", "AvionID", "PuertaID", "Salida", "Llegada Est.", "Estado");
        foreach (var f in lista)
            SpectreHelper.AgregarFila(tabla,
                f.Id.ToString(),
                f.NumeroVuelo.Valor,
                f.RutaId.ToString(),
                f.AvionId.ToString(),
                f.PuertaEmbarqueId?.ToString() ?? "-",
                f.FechaSalida.Valor.ToString("yyyy-MM-dd HH:mm"),
                f.FechaLlegadaEstimada.Valor.ToString("yyyy-MM-dd HH:mm"),
                f.Estado.Valor);
        SpectreHelper.MostrarTabla(tabla);
    }

    private async Task MostrarRutasActivasAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetActiveRoutesUseCase>().ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "AerolineaID", "OrigenID", "DestinoID");
            foreach (var r in lista)
                SpectreHelper.AgregarFila(tabla,
                    r.Id.ToString(), r.AerolineaId.ToString(),
                    r.OrigenId.ToString(), r.DestinoId.ToString());
            SpectreHelper.MostrarTabla(tabla);
        });
    }

    private async Task MostrarAvionesDisponiblesAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAvailableAircraftUseCase>().ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "Matrícula", "ModeloID", "AerolineaID");
            foreach (var a in lista)
                SpectreHelper.AgregarFila(tabla,
                    a.Id.ToString(), a.Matricula.Valor,
                    a.ModeloAvionId.ToString(), a.AerolineaId.ToString());
            SpectreHelper.MostrarTabla(tabla);
        });
    }
}
