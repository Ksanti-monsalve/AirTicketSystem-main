// src/UI/Admin/AirportsRoutes/RouteMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.route.Application.UseCases;
using AirTicketSystem.modules.airline.Application.UseCases;
using AirTicketSystem.modules.airport.Application.UseCases;

namespace AirTicketSystem.UI.Admin.AirportsRoutes;

public sealed class RouteMenu
{
    private readonly IServiceProvider _provider;

    public RouteMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Rutas");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Listar todas",
                    "Listar activas",
                    "Buscar por origen y destino",
                    "Listar por aerolínea",
                    "Crear",
                    "Editar",
                    "Activar",
                    "Desactivar",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Listar todas":               await ListarTodasAsync();        break;
                case "Listar activas":             await ListarActivasAsync();      break;
                case "Buscar por origen y destino": await BuscarAsync();            break;
                case "Listar por aerolínea":       await ListarPorAerolineaAsync(); break;
                case "Crear":                      await CrearAsync();              break;
                case "Editar":                     await EditarAsync();             break;
                case "Activar":                    await ActivarAsync();            break;
                case "Desactivar":                 await DesactivarAsync();         break;
                case "Volver":                     return;
            }
        }
    }

    private async Task ListarTodasAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllRoutesUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin rutas."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "AerolineaID", "OrigenID", "DestinoID", "Dist(km)", "Duración", "Activa");
            foreach (var r in lista)
                SpectreHelper.AgregarFila(tabla,
                    r.Id.ToString(), r.AerolineaId.ToString(),
                    r.OrigenId.ToString(), r.DestinoId.ToString(),
                    r.DistanciaKm?.Valor.ToString() ?? "-",
                    r.DuracionEstimadaMin?.EnFormatoHorasMinutos ?? "-",
                    r.Activa.Valor ? "Sí" : "No");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarActivasAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetActiveRoutesUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("No hay rutas activas."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "AerolineaID", "OrigenID", "DestinoID", "Dist(km)", "Duración");
            foreach (var r in lista)
                SpectreHelper.AgregarFila(tabla,
                    r.Id.ToString(), r.AerolineaId.ToString(),
                    r.OrigenId.ToString(), r.DestinoId.ToString(),
                    r.DistanciaKm?.Valor.ToString() ?? "-",
                    r.DuracionEstimadaMin?.EnFormatoHorasMinutos ?? "-");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task BuscarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Buscar Ruta");
        await MostrarAeropuertosAsync();
        var origenId  = SpectreHelper.PedirEntero("ID del aeropuerto de origen");
        var destinoId = SpectreHelper.PedirEntero("ID del aeropuerto de destino");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<SearchRoutesUseCase>()
                .ExecuteAsync(origenId, destinoId);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin resultados."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "AerolineaID", "Dist(km)", "Duración", "Activa");
            foreach (var r in lista)
                SpectreHelper.AgregarFila(tabla,
                    r.Id.ToString(), r.AerolineaId.ToString(),
                    r.DistanciaKm?.Valor.ToString() ?? "-",
                    r.DuracionEstimadaMin?.EnFormatoHorasMinutos ?? "-",
                    r.Activa.Valor ? "Sí" : "No");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarPorAerolineaAsync()
    {
        await MostrarAerolineasAsync();
        var aerolineaId = SpectreHelper.PedirEntero("ID de la aerolínea");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetRoutesByAirlineUseCase>()
                .ExecuteAsync(aerolineaId);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin rutas para esa aerolínea."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "OrigenID", "DestinoID", "Dist(km)", "Duración", "Activa");
            foreach (var r in lista)
                SpectreHelper.AgregarFila(tabla,
                    r.Id.ToString(), r.OrigenId.ToString(), r.DestinoId.ToString(),
                    r.DistanciaKm?.Valor.ToString() ?? "-",
                    r.DuracionEstimadaMin?.EnFormatoHorasMinutos ?? "-",
                    r.Activa.Valor ? "Sí" : "No");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nueva Ruta");
        await MostrarAerolineasAsync();
        var aerolineaId = SpectreHelper.PedirEntero("ID de la aerolínea");

        await MostrarAeropuertosAsync();
        var origenId  = SpectreHelper.PedirEntero("ID del aeropuerto de origen");
        var destinoId = SpectreHelper.PedirEntero("ID del aeropuerto de destino");

        var distStr = SpectreHelper.PedirTexto("Distancia en km (opcional, Enter para omitir)");
        var durStr  = SpectreHelper.PedirTexto("Duración estimada en minutos (opcional, Enter para omitir)");
        int? distancia = int.TryParse(distStr, out var dk) && dk > 0 ? dk : null;
        int? duracion  = int.TryParse(durStr,  out var dm) && dm > 0 ? dm : null;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<CreateRouteUseCase>()
                .ExecuteAsync(aerolineaId, origenId, destinoId, distancia, duracion);
            SpectreHelper.MostrarExito($"Ruta creada (ID {r.Id}): aerolínea {r.AerolineaId}, {r.OrigenId} → {r.DestinoId}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Editar Ruta");
        var id      = SpectreHelper.PedirEntero("ID de la ruta");
        var distStr = SpectreHelper.PedirTexto("Nueva distancia en km (opcional, Enter para limpiar)");
        var durStr  = SpectreHelper.PedirTexto("Nueva duración en minutos (opcional, Enter para limpiar)");
        int? distancia = int.TryParse(distStr, out var dk) && dk > 0 ? dk : null;
        int? duracion  = int.TryParse(durStr,  out var dm) && dm > 0 ? dm : null;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<UpdateRouteUseCase>()
                .ExecuteAsync(id, distancia, duracion);
            SpectreHelper.MostrarExito($"Ruta {r.Id} actualizada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ActivarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la ruta a activar");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<ActivateRouteUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Ruta activada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task DesactivarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la ruta a desactivar");
        if (!SpectreHelper.Confirmar("¿Confirma desactivar?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeactivateRouteUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Ruta desactivada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task MostrarAerolineasAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetActiveAirlinesUseCase>().ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "IATA");
            foreach (var a in lista)
                SpectreHelper.AgregarFila(tabla, a.Id.ToString(), a.Nombre.Valor, a.CodigoIata.Valor);
            SpectreHelper.MostrarTabla(tabla);
        });
    }

    private async Task MostrarAeropuertosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetActiveAirportsUseCase>().ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "IATA");
            foreach (var a in lista)
                SpectreHelper.AgregarFila(tabla, a.Id.ToString(), a.Nombre.Valor, a.CodigoIata.Valor);
            SpectreHelper.MostrarTabla(tabla);
        });
    }
}
