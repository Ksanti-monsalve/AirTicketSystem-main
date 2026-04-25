// src/UI/Admin/GeoConfig/RegionMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.region.Application.UseCases;

namespace AirTicketSystem.UI.Admin.GeoConfig;

public sealed class RegionMenu
{
    private readonly IServiceProvider _provider;

    public RegionMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Regiones");
            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                ["Listar todas", "Listar por país", "Crear", "Editar", "Eliminar", "Volver"]);

            switch (opcion)
            {
                case "Listar todas":   await ListarTodasAsync();   break;
                case "Listar por país": await ListarPorPaisAsync(); break;
                case "Crear":          await CrearAsync();         break;
                case "Editar":         await EditarAsync();        break;
                case "Eliminar":       await EliminarAsync();      break;
                case "Volver":         return;
            }
        }
    }

    private async Task ListarTodasAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllRegionsUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin regiones."); SpectreHelper.EsperarTecla(); return; }
            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "PaísID");
            foreach (var r in lista) SpectreHelper.AgregarFila(tabla, r.Id.ToString(), r.Nombre.Valor, r.PaisId.ToString());
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
            var lista = await scope.ServiceProvider.GetRequiredService<GetRegionsByCountryUseCase>().ExecuteAsync(pais.Id);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo($"Sin regiones en {pais.Nombre.Valor}."); SpectreHelper.EsperarTecla(); return; }
            var tabla = SpectreHelper.CrearTabla("ID", "Nombre");
            foreach (var r in lista) SpectreHelper.AgregarFila(tabla, r.Id.ToString(), r.Nombre.Valor);
            SpectreHelper.MostrarTabla(tabla); SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nueva Región");
        var pais = await SelectorUI.SeleccionarPaisAsync(_provider);
        if (pais is null) return;
        var nombre = SpectreHelper.PedirTexto("Nombre de la región");
        var codigo = SpectreHelper.PedirTexto("Código de la región (opcional)", obligatorio: false);
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<CreateRegionUseCase>()
                .ExecuteAsync(pais.Id, nombre, string.IsNullOrWhiteSpace(codigo) ? null : codigo);
            SpectreHelper.MostrarExito($"Región '{r.Nombre.Valor}' creada en {pais.Nombre.Valor} (ID {r.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        var region = await SelectorUI.SeleccionarRegionAsync(_provider);
        if (region is null) return;
        SpectreHelper.MostrarSubtitulo($"Editando: {region.Nombre.Valor}");
        var nombre = SpectreHelper.PedirTexto($"Nuevo nombre  (actual: {region.Nombre.Valor})");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var codigo = SpectreHelper.PedirTexto("Nuevo código (actual: " + (region.Codigo?.Valor ?? "") + ", Enter para omitir)", obligatorio: false);
            var r = await scope.ServiceProvider.GetRequiredService<UpdateRegionUseCase>().ExecuteAsync(region.Id, nombre, string.IsNullOrWhiteSpace(codigo) ? null : codigo, default);
            SpectreHelper.MostrarExito($"Región '{r.Nombre.Valor}' actualizada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        var region = await SelectorUI.SeleccionarRegionAsync(_provider);
        if (region is null) return;
        if (!SpectreHelper.Confirmar($"¿Eliminar '{region.Nombre.Valor}'?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeleteRegionUseCase>().ExecuteAsync(region.Id);
            SpectreHelper.MostrarExito($"Región '{region.Nombre.Valor}' eliminada.");
        });
        SpectreHelper.EsperarTecla();
    }
}
