// src/UI/Admin/Aeronautica/AircraftModelMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.aircraftmodel.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Aeronautica;

public sealed class AircraftModelMenu
{
    private readonly IServiceProvider _provider;

    public AircraftModelMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Modelos de Aeronave");
            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                ["Listar todos", "Listar por fabricante", "Crear", "Editar", "Eliminar", "Volver"]);

            switch (opcion)
            {
                case "Listar todos":          await ListarTodosAsync();         break;
                case "Listar por fabricante": await ListarPorFabricanteAsync(); break;
                case "Crear":                 await CrearAsync();               break;
                case "Editar":                await EditarAsync();              break;
                case "Eliminar":              await EliminarAsync();            break;
                case "Volver":                return;
            }
        }
    }

    private async Task ListarTodosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllAircraftModelsUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin modelos registrados."); SpectreHelper.EsperarTecla(); return; }
            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "Código", "FabricanteID", "Autonomía(km)", "Velocidad(km/h)");
            foreach (var m in lista)
                SpectreHelper.AgregarFila(tabla, m.Id.ToString(), m.Nombre.Valor, m.CodigoModelo.Valor,
                    m.FabricanteId.ToString(), m.AutonomiKm?.Valor.ToString() ?? "-", m.VelocidadCruceroKmh?.Valor.ToString() ?? "-");
            SpectreHelper.MostrarTabla(tabla); SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarPorFabricanteAsync()
    {
        var fab = await SelectorUI.SeleccionarFabricanteAsync(_provider);
        if (fab is null) return;
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAircraftModelsByManufacturerUseCase>().ExecuteAsync(fab.Id);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo($"Sin modelos de {fab.Nombre.Valor}."); SpectreHelper.EsperarTecla(); return; }
            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "Código", "Autonomía(km)");
            foreach (var m in lista)
                SpectreHelper.AgregarFila(tabla, m.Id.ToString(), m.Nombre.Valor, m.CodigoModelo.Valor, m.AutonomiKm?.Valor.ToString() ?? "-");
            SpectreHelper.MostrarTabla(tabla); SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nuevo Modelo de Aeronave");
        var fab = await SelectorUI.SeleccionarFabricanteAsync(_provider);
        if (fab is null) return;

        var nombre      = SpectreHelper.PedirTexto("Nombre del modelo (ej: A320neo)");
        var codigo      = SpectreHelper.PedirTexto("Código del modelo (ej: A320)");
        var autonomiaStr= SpectreHelper.PedirTexto("Autonomía en km (opcional)", obligatorio: false);
        var velocidadStr= SpectreHelper.PedirTexto("Velocidad crucero km/h (opcional)", obligatorio: false);
        var descripcion = SpectreHelper.PedirTexto("Descripción (opcional)", obligatorio: false);

        int? autonomia  = int.TryParse(autonomiaStr, out var a) && a > 0 ? a : null;
        int? velocidad  = int.TryParse(velocidadStr, out var v) && v > 0 ? v : null;
        string? descOpc = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<CreateAircraftModelUseCase>()
                .ExecuteAsync(fab.Id, nombre, codigo, autonomia, velocidad, descOpc);
            SpectreHelper.MostrarExito($"Modelo '{r.Nombre.Valor}' creado para {fab.Nombre.Valor} (ID {r.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        var modelo = await SelectorUI.SeleccionarModeloAvionAsync(_provider);
        if (modelo is null) return;

        SpectreHelper.MostrarSubtitulo($"Editando: {modelo.Nombre.Valor} [{modelo.CodigoModelo.Valor}]");
        var nombre      = SpectreHelper.PedirTexto($"Nuevo nombre  (actual: {modelo.Nombre.Valor})");
        var autonomiaStr= SpectreHelper.PedirTexto($"Nueva autonomía km  (actual: {modelo.AutonomiKm?.Valor.ToString() ?? "no registrada"})", obligatorio: false);
        var velocidadStr= SpectreHelper.PedirTexto($"Nueva velocidad km/h  (actual: {modelo.VelocidadCruceroKmh?.Valor.ToString() ?? "no registrada"})", obligatorio: false);
        var descripcion = SpectreHelper.PedirTexto("Nueva descripción (opcional)", obligatorio: false);

        int? autonomia  = int.TryParse(autonomiaStr, out var a) && a > 0 ? a : null;
        int? velocidad  = int.TryParse(velocidadStr, out var v) && v > 0 ? v : null;
        string? descOpc = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<UpdateAircraftModelUseCase>()
                .ExecuteAsync(modelo.Id, nombre, autonomia, velocidad, descOpc);
            SpectreHelper.MostrarExito($"Modelo '{r.Nombre.Valor}' actualizado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        var modelo = await SelectorUI.SeleccionarModeloAvionAsync(_provider);
        if (modelo is null) return;
        if (!SpectreHelper.Confirmar($"¿Eliminar modelo '{modelo.Nombre.Valor}'?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeleteAircraftModelUseCase>().ExecuteAsync(modelo.Id);
            SpectreHelper.MostrarExito($"Modelo '{modelo.Nombre.Valor}' eliminado.");
        });
        SpectreHelper.EsperarTecla();
    }
}
