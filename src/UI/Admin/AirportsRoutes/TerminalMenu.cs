// src/UI/Admin/AirportsRoutes/TerminalMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.terminal.Application.UseCases;

namespace AirTicketSystem.UI.Admin.AirportsRoutes;

public sealed class TerminalMenu
{
    private readonly IServiceProvider _provider;

    public TerminalMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Terminales");
            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                ["Ver terminales de aeropuerto", "Crear", "Editar", "Eliminar", "Volver"]);

            switch (opcion)
            {
                case "Ver terminales de aeropuerto": await ListarAsync();   break;
                case "Crear":                        await CrearAsync();    break;
                case "Editar":                       await EditarAsync();   break;
                case "Eliminar":                     await EliminarAsync(); break;
                case "Volver":                       return;
            }
        }
    }

    private async Task ListarAsync()
    {
        var aeropuerto = await SelectorUI.SeleccionarAeropuertoAsync(_provider);
        if (aeropuerto is null) return;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetTerminalsByAirportUseCase>().ExecuteAsync(aeropuerto.Id);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo($"Sin terminales en {aeropuerto.Nombre.Valor}."); SpectreHelper.EsperarTecla(); return; }
            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "Descripción");
            foreach (var t in lista)
                SpectreHelper.AgregarFila(tabla, t.Id.ToString(), t.Nombre.Valor, t.Descripcion?.Valor ?? "-");
            SpectreHelper.MostrarTabla(tabla); SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nueva Terminal");
        var aeropuerto = await SelectorUI.SeleccionarAeropuertoAsync(_provider);
        if (aeropuerto is null) return;

        var nombre     = SpectreHelper.PedirTexto("Nombre de la terminal (ej: T1 Nacional)");
        var descripcion= SpectreHelper.PedirTexto("Descripción (opcional)", obligatorio: false);

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<CreateTerminalUseCase>()
                .ExecuteAsync(aeropuerto.Id, nombre, string.IsNullOrWhiteSpace(descripcion) ? null : descripcion);
            SpectreHelper.MostrarExito($"Terminal '{r.Nombre.Valor}' creada en {aeropuerto.Nombre.Valor} (ID {r.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        // Seleccionar aeropuerto → terminal
        var aeropuerto = await SelectorUI.SeleccionarAeropuertoAsync(_provider);
        if (aeropuerto is null) return;
        var terminal = await SelectorUI.SeleccionarTerminalAsync(_provider, aeropuerto.Id);
        if (terminal is null) return;

        SpectreHelper.MostrarSubtitulo($"Editando: {terminal.Nombre.Valor}");
        var nombre     = SpectreHelper.PedirTexto($"Nuevo nombre  (actual: {terminal.Nombre.Valor})");
        var descripcion= SpectreHelper.PedirTexto($"Nueva descripción  (actual: {terminal.Descripcion?.Valor ?? "-"})", obligatorio: false);

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<UpdateTerminalUseCase>()
                .ExecuteAsync(terminal.Id, nombre, string.IsNullOrWhiteSpace(descripcion) ? null : descripcion);
            SpectreHelper.MostrarExito($"Terminal '{r.Nombre.Valor}' actualizada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        var aeropuerto = await SelectorUI.SeleccionarAeropuertoAsync(_provider);
        if (aeropuerto is null) return;
        var terminal = await SelectorUI.SeleccionarTerminalAsync(_provider, aeropuerto.Id);
        if (terminal is null) return;

        if (!SpectreHelper.Confirmar($"¿Eliminar terminal '{terminal.Nombre.Valor}'?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeleteTerminalUseCase>().ExecuteAsync(terminal.Id);
            SpectreHelper.MostrarExito($"Terminal '{terminal.Nombre.Valor}' eliminada.");
        });
        SpectreHelper.EsperarTecla();
    }
}
