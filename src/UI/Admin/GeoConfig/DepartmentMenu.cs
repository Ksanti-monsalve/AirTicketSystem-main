// src/UI/Admin/GeoConfig/DepartmentMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.department.Application.UseCases;

namespace AirTicketSystem.UI.Admin.GeoConfig;

public sealed class DepartmentMenu
{
    private readonly IServiceProvider _provider;

    public DepartmentMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Departamentos / Provincias");
            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                ["Listar todos", "Listar por región", "Crear", "Editar", "Eliminar", "Volver"]);

            switch (opcion)
            {
                case "Listar todos":    await ListarTodosAsync();    break;
                case "Listar por región": await ListarPorRegionAsync(); break;
                case "Crear":           await CrearAsync();          break;
                case "Editar":          await EditarAsync();         break;
                case "Eliminar":        await EliminarAsync();       break;
                case "Volver":          return;
            }
        }
    }

    private async Task ListarTodosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllDepartmentsUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin departamentos."); SpectreHelper.EsperarTecla(); return; }
            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "RegiónID");
            foreach (var d in lista) SpectreHelper.AgregarFila(tabla, d.Id.ToString(), d.Nombre.Valor, d.RegionId.ToString());
            SpectreHelper.MostrarTabla(tabla); SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarPorRegionAsync()
    {
        var region = await SelectorUI.SeleccionarRegionAsync(_provider);
        if (region is null) return;
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetDepartmentsByRegionUseCase>().ExecuteAsync(region.Id);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo($"Sin departamentos en {region.Nombre.Valor}."); SpectreHelper.EsperarTecla(); return; }
            var tabla = SpectreHelper.CrearTabla("ID", "Nombre");
            foreach (var d in lista) SpectreHelper.AgregarFila(tabla, d.Id.ToString(), d.Nombre.Valor);
            SpectreHelper.MostrarTabla(tabla); SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nuevo Departamento / Provincia");
        var region = await SelectorUI.SeleccionarRegionAsync(_provider);
        if (region is null) return;
        var nombre = SpectreHelper.PedirTexto("Nombre del departamento");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var codigo = SpectreHelper.PedirTexto("Código del departamento (opcional, Enter para omitir)", obligatorio: false);
            var d = await scope.ServiceProvider.GetRequiredService<CreateDepartmentUseCase>().ExecuteAsync(region.Id, nombre, string.IsNullOrWhiteSpace(codigo) ? null : codigo, default);
            SpectreHelper.MostrarExito($"Departamento '{d.Nombre.Valor}' creado en {region.Nombre.Valor} (ID {d.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        var dept = await SelectorUI.SeleccionarDepartamentoAsync(_provider);
        if (dept is null) return;
        SpectreHelper.MostrarSubtitulo($"Editando: {dept.Nombre.Valor}");
        var nombre = SpectreHelper.PedirTexto($"Nuevo nombre  (actual: {dept.Nombre.Valor})");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var codigo = SpectreHelper.PedirTexto("Nuevo código (actual: " + (dept.Codigo?.Valor ?? "") + ", Enter para omitir)", obligatorio: false);
            var d = await scope.ServiceProvider.GetRequiredService<UpdateDepartmentUseCase>().ExecuteAsync(dept.Id, nombre, string.IsNullOrWhiteSpace(codigo) ? null : codigo, default);
            SpectreHelper.MostrarExito($"Departamento '{d.Nombre.Valor}' actualizado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        var dept = await SelectorUI.SeleccionarDepartamentoAsync(_provider);
        if (dept is null) return;
        if (!SpectreHelper.Confirmar($"¿Eliminar '{dept.Nombre.Valor}'?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeleteDepartmentUseCase>().ExecuteAsync(dept.Id);
            SpectreHelper.MostrarExito($"Departamento '{dept.Nombre.Valor}' eliminado.");
        });
        SpectreHelper.EsperarTecla();
    }
}
