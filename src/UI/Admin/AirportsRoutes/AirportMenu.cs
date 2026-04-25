// src/UI/Admin/AirportsRoutes/AirportMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.airport.Application.UseCases;

namespace AirTicketSystem.UI.Admin.AirportsRoutes;

public sealed class AirportMenu
{
    private readonly IServiceProvider _provider;

    public AirportMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Aeropuertos");
            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                ["Listar todos", "Listar activos", "Crear", "Editar", "Activar", "Desactivar", "Volver"]);

            switch (opcion)
            {
                case "Listar todos":   await ListarTodosAsync();  break;
                case "Listar activos": await ListarActivosAsync(); break;
                case "Crear":          await CrearAsync();         break;
                case "Editar":         await EditarAsync();        break;
                case "Activar":        await ActivarAsync();       break;
                case "Desactivar":     await DesactivarAsync();    break;
                case "Volver":         return;
            }
        }
    }

    private async Task ListarTodosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllAirportsUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin aeropuertos."); SpectreHelper.EsperarTecla(); return; }
            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "IATA", "ICAO", "Ciudad ID", "Activo");
            foreach (var a in lista)
                SpectreHelper.AgregarFila(tabla, a.Id.ToString(), a.Nombre.Valor,
                    a.CodigoIata.Valor, a.CodigoIcao.Valor, a.CiudadId.ToString(), a.Activo.Valor ? "Sí" : "No");
            SpectreHelper.MostrarTabla(tabla); SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarActivosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetActiveAirportsUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin aeropuertos activos."); SpectreHelper.EsperarTecla(); return; }
            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "IATA", "ICAO", "Ciudad ID");
            foreach (var a in lista)
                SpectreHelper.AgregarFila(tabla, a.Id.ToString(), a.Nombre.Valor,
                    a.CodigoIata.Valor, a.CodigoIcao.Valor, a.CiudadId.ToString());
            SpectreHelper.MostrarTabla(tabla); SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nuevo Aeropuerto");
        var ciudad = await SelectorUI.SeleccionarCiudadAsync(_provider);
        if (ciudad is null) return;

        var iata     = SpectreHelper.PedirTexto("Código IATA — 3 letras (ej: BOG)");
        var icao     = SpectreHelper.PedirTexto("Código ICAO — 4 letras (ej: SKBO)");
        var nombre   = SpectreHelper.PedirTexto("Nombre del aeropuerto");
        var direccion= SpectreHelper.PedirTexto("Dirección (opcional)", obligatorio: false);

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<CreateAirportUseCase>()
                .ExecuteAsync(ciudad.Id, iata, icao, nombre,
                    string.IsNullOrWhiteSpace(direccion) ? null : direccion);
            SpectreHelper.MostrarExito($"Aeropuerto '{r.Nombre.Valor}' [{r.CodigoIata.Valor}] creado en {ciudad.Nombre.Valor} (ID {r.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        var aeropuerto = await SelectorUI.SeleccionarAeropuertoAsync(_provider);
        if (aeropuerto is null) return;

        SpectreHelper.MostrarSubtitulo($"Editando: {aeropuerto.Nombre.Valor} [{aeropuerto.CodigoIata.Valor}]");
        var nombre   = SpectreHelper.PedirTexto($"Nuevo nombre  (actual: {aeropuerto.Nombre.Valor})");
        var direccion= SpectreHelper.PedirTexto($"Nueva dirección (actual: {aeropuerto.Direccion?.Valor ?? "-"})", obligatorio: false);

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<UpdateAirportUseCase>()
                .ExecuteAsync(aeropuerto.Id, nombre,
                    string.IsNullOrWhiteSpace(direccion) ? null : direccion);
            SpectreHelper.MostrarExito($"Aeropuerto '{r.Nombre.Valor}' actualizado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ActivarAsync()
    {
        var aeropuerto = await SelectorUI.SeleccionarAeropuertoAsync(_provider);
        if (aeropuerto is null) return;
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<ActivateAirportUseCase>().ExecuteAsync(aeropuerto.Id);
            SpectreHelper.MostrarExito($"Aeropuerto '{aeropuerto.Nombre.Valor}' activado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task DesactivarAsync()
    {
        var aeropuerto = await SelectorUI.SeleccionarAeropuertoAsync(_provider);
        if (aeropuerto is null) return;
        if (!SpectreHelper.Confirmar($"¿Desactivar '{aeropuerto.Nombre.Valor}'?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeactivateAirportUseCase>().ExecuteAsync(aeropuerto.Id);
            SpectreHelper.MostrarExito($"Aeropuerto '{aeropuerto.Nombre.Valor}' desactivado.");
        });
        SpectreHelper.EsperarTecla();
    }
}
