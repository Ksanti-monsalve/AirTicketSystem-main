// src/UI/Admin/GeoConfig/CountryMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.country.Application.UseCases;

namespace AirTicketSystem.UI.Admin.GeoConfig;

public sealed class CountryMenu
{
    private readonly IServiceProvider _provider;

    public CountryMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Países");
            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                ["Listar todos", "Listar por continente", "Crear", "Editar", "Eliminar", "Volver"]);

            switch (opcion)
            {
                case "Listar todos":          await ListarTodosAsync();        break;
                case "Listar por continente": await ListarPorContinenteAsync(); break;
                case "Crear":                 await CrearAsync();              break;
                case "Editar":                await EditarAsync();             break;
                case "Eliminar":              await EliminarAsync();           break;
                case "Volver":                return;
            }
        }
    }

    private async Task ListarTodosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllCountriesUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin países registrados."); SpectreHelper.EsperarTecla(); return; }
            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "ISO2", "ISO3", "ContinenteID");
            foreach (var c in lista)
                SpectreHelper.AgregarFila(tabla, c.Id.ToString(), c.Nombre.Valor,
                    c.CodigoIso2.Valor, c.CodigoIso3.Valor, c.ContinenteId.ToString());
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarPorContinenteAsync()
    {
        var cont = await SelectorUI.SeleccionarContinenteAsync(_provider);
        if (cont is null) return;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetCountriesByContinentUseCase>().ExecuteAsync(cont.Id);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo($"Sin países en {cont.Nombre.Valor}."); SpectreHelper.EsperarTecla(); return; }
            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "ISO2", "ISO3");
            foreach (var c in lista)
                SpectreHelper.AgregarFila(tabla, c.Id.ToString(), c.Nombre.Valor, c.CodigoIso2.Valor, c.CodigoIso3.Valor);
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nuevo País");
        var cont = await SelectorUI.SeleccionarContinenteAsync(_provider);
        if (cont is null) return;

        var nombre = SpectreHelper.PedirTexto("Nombre del país");
        var iso2   = SpectreHelper.PedirTexto("Código ISO2 — 2 letras (ej: CO)");
        var iso3   = SpectreHelper.PedirTexto("Código ISO3 — 3 letras (ej: COL)");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<CreateCountryUseCase>()
                .ExecuteAsync(cont.Id, nombre, iso2, iso3);
            SpectreHelper.MostrarExito($"País '{r.Nombre.Valor}' creado (ID {r.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        var pais = await SelectorUI.SeleccionarPaisAsync(_provider);
        if (pais is null) return;

        SpectreHelper.MostrarSubtitulo($"Editando: {pais.Nombre.Valor} [{pais.CodigoIso2.Valor}]");
        var nombre = SpectreHelper.PedirTexto($"Nuevo nombre  (actual: {pais.Nombre.Valor})");
        var iso2   = SpectreHelper.PedirTexto($"Nuevo ISO2    (actual: {pais.CodigoIso2.Valor})");
        var iso3   = SpectreHelper.PedirTexto($"Nuevo ISO3    (actual: {pais.CodigoIso3.Valor})");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<UpdateCountryUseCase>()
                .ExecuteAsync(pais.Id, nombre, iso2, iso3);
            SpectreHelper.MostrarExito($"País '{r.Nombre.Valor}' actualizado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        var pais = await SelectorUI.SeleccionarPaisAsync(_provider);
        if (pais is null) return;

        if (!SpectreHelper.Confirmar($"¿Eliminar '{pais.Nombre.Valor}'?")) { SpectreHelper.EsperarTecla(); return; }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeleteCountryUseCase>().ExecuteAsync(pais.Id);
            SpectreHelper.MostrarExito($"País '{pais.Nombre.Valor}' eliminado.");
        });
        SpectreHelper.EsperarTecla();
    }
}
