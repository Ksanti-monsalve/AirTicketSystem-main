// src/UI/Admin/GeoConfig/CityMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.city.Application.UseCases;

namespace AirTicketSystem.UI.Admin.GeoConfig;

public sealed class CityMenu
{
    private readonly IServiceProvider _provider;

    public CityMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Ciudades");
            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                ["Listar todas", "Listar por región/dpto", "Crear", "Editar", "Eliminar", "Volver"]);

            switch (opcion)
            {
                case "Listar todas":        await ListarTodasAsync();   break;
                case "Listar por región/dpto": await ListarPorDeptAsync(); break;
                case "Crear":               await CrearAsync();         break;
                case "Editar":              await EditarAsync();        break;
                case "Eliminar":            await EliminarAsync();      break;
                case "Volver":              return;
            }
        }
    }

    private async Task ListarTodasAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllCitiesUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin ciudades."); SpectreHelper.EsperarTecla(); return; }
            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "DepartamentoID");
            foreach (var c in lista) SpectreHelper.AgregarFila(tabla, c.Id.ToString(), c.Nombre.Valor, c.DepartamentoId.ToString());
            SpectreHelper.MostrarTabla(tabla); SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarPorDeptAsync()
    {
        var dept = await SelectorUI.SeleccionarDepartamentoAsync(_provider);
        if (dept is null) return;
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetCitiesByDepartmentUseCase>().ExecuteAsync(dept.Id);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo($"Sin ciudades en {dept.Nombre.Valor}."); SpectreHelper.EsperarTecla(); return; }
            var tabla = SpectreHelper.CrearTabla("ID", "Nombre");
            foreach (var c in lista) SpectreHelper.AgregarFila(tabla, c.Id.ToString(), c.Nombre.Valor);
            SpectreHelper.MostrarTabla(tabla); SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nueva Ciudad");
        var dept = await SelectorUI.SeleccionarDepartamentoAsync(_provider);
        if (dept is null) return;
        var nombre = SpectreHelper.PedirTexto("Nombre de la ciudad");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var codigoPostal = SpectreHelper.PedirTexto("Código postal (opcional, Enter para omitir)", obligatorio: false);
            var c = await scope.ServiceProvider.GetRequiredService<CreateCityUseCase>().ExecuteAsync(dept.Id, nombre, string.IsNullOrWhiteSpace(codigoPostal) ? null : codigoPostal, default);
            SpectreHelper.MostrarExito($"Ciudad '{c.Nombre.Valor}' creada en {dept.Nombre.Valor} (ID {c.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        var ciudad = await SelectorUI.SeleccionarCiudadAsync(_provider);
        if (ciudad is null) return;
        SpectreHelper.MostrarSubtitulo($"Editando: {ciudad.Nombre.Valor}");
        var nombre = SpectreHelper.PedirTexto($"Nuevo nombre  (actual: {ciudad.Nombre.Valor})");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var codigoPostal = SpectreHelper.PedirTexto("Nuevo código postal (actual: " + (ciudad.CodigoPostal?.Valor ?? "") + ", Enter para omitir)", obligatorio: false);
            var c = await scope.ServiceProvider.GetRequiredService<UpdateCityUseCase>().ExecuteAsync(ciudad.Id, nombre, string.IsNullOrWhiteSpace(codigoPostal) ? null : codigoPostal, default);
            SpectreHelper.MostrarExito($"Ciudad '{c.Nombre.Valor}' actualizada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        var ciudad = await SelectorUI.SeleccionarCiudadAsync(_provider);
        if (ciudad is null) return;
        if (!SpectreHelper.Confirmar($"¿Eliminar '{ciudad.Nombre.Valor}'?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeleteCityUseCase>().ExecuteAsync(ciudad.Id);
            SpectreHelper.MostrarExito($"Ciudad '{ciudad.Nombre.Valor}' eliminada.");
        });
        SpectreHelper.EsperarTecla();
    }
}
