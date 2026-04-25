// src/UI/Admin/Aeronautica/AircraftManufacturerMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.aircraftmanufacturer.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Aeronautica;

public sealed class AircraftManufacturerMenu
{
    private readonly IServiceProvider _provider;

    public AircraftManufacturerMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Fabricantes de Aeronaves");
            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                ["Listar todos", "Listar por país", "Crear", "Editar", "Eliminar", "Volver"]);

            switch (opcion)
            {
                case "Listar todos":    await ListarTodosAsync();   break;
                case "Listar por país": await ListarPorPaisAsync(); break;
                case "Crear":           await CrearAsync();         break;
                case "Editar":          await EditarAsync();        break;
                case "Eliminar":        await EliminarAsync();      break;
                case "Volver":          return;
            }
        }
    }

    private async Task ListarTodosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllAircraftManufacturersUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin registros."); SpectreHelper.EsperarTecla(); return; }
            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "PaísID", "Sitio Web");
            foreach (var f in lista)
                SpectreHelper.AgregarFila(tabla, f.Id.ToString(), f.Nombre.Valor,
                    f.PaisId.ToString(), f.SitioWeb?.Valor ?? "-");
            SpectreHelper.MostrarTabla(tabla); SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarPorPaisAsync()
    {
        var pais = await SelectorUI.SeleccionarPaisAsync(_provider);
        if (pais is null) return;
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAircraftManufacturersByCountryUseCase>().ExecuteAsync(pais.Id);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo($"Sin fabricantes en {pais.Nombre.Valor}."); SpectreHelper.EsperarTecla(); return; }
            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "Sitio Web");
            foreach (var f in lista) SpectreHelper.AgregarFila(tabla, f.Id.ToString(), f.Nombre.Valor, f.SitioWeb?.Valor ?? "-");
            SpectreHelper.MostrarTabla(tabla); SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nuevo Fabricante");
        var pais = await SelectorUI.SeleccionarPaisAsync(_provider);
        if (pais is null) return;

        var nombre = SpectreHelper.PedirTexto("Nombre del fabricante");
        var web    = SpectreHelper.PedirTexto("Sitio web (opcional, Enter para omitir)", obligatorio: false);
        string? webOpc = string.IsNullOrWhiteSpace(web) ? null : web;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<CreateAircraftManufacturerUseCase>()
                .ExecuteAsync(pais.Id, nombre, webOpc);
            SpectreHelper.MostrarExito($"Fabricante '{r.Nombre.Valor}' creado en {pais.Nombre.Valor} (ID {r.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        var fab = await SelectorUI.SeleccionarFabricanteAsync(_provider);
        if (fab is null) return;

        SpectreHelper.MostrarSubtitulo($"Editando: {fab.Nombre.Valor}");
        var nombre = SpectreHelper.PedirTexto($"Nuevo nombre  (actual: {fab.Nombre.Valor})");
        var web    = SpectreHelper.PedirTexto($"Nuevo sitio web  (actual: {fab.SitioWeb?.Valor ?? "-"})", obligatorio: false);
        string? webOpc = string.IsNullOrWhiteSpace(web) ? null : web;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<UpdateAircraftManufacturerUseCase>()
                .ExecuteAsync(fab.Id, nombre, webOpc);
            SpectreHelper.MostrarExito($"Fabricante '{r.Nombre.Valor}' actualizado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        var fab = await SelectorUI.SeleccionarFabricanteAsync(_provider);
        if (fab is null) return;
        if (!SpectreHelper.Confirmar($"¿Eliminar '{fab.Nombre.Valor}'?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeleteAircraftManufacturerUseCase>().ExecuteAsync(fab.Id);
            SpectreHelper.MostrarExito($"Fabricante '{fab.Nombre.Valor}' eliminado.");
        });
        SpectreHelper.EsperarTecla();
    }
}
