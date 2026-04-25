// src/UI/Admin/AirportsRoutes/GateMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.gate.Application.UseCases;

namespace AirTicketSystem.UI.Admin.AirportsRoutes;

public sealed class GateMenu
{
    private readonly IServiceProvider _provider;

    public GateMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Puertas de Embarque");
            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                ["Listar por terminal", "Listar activas por terminal", "Crear", "Editar", "Activar", "Desactivar", "Volver"]);

            switch (opcion)
            {
                case "Listar por terminal":         await ListarPorTerminalAsync();        break;
                case "Listar activas por terminal": await ListarActivasPorTerminalAsync(); break;
                case "Crear":                       await CrearAsync();                    break;
                case "Editar":                      await EditarAsync();                   break;
                case "Activar":                     await ActivarAsync();                  break;
                case "Desactivar":                  await DesactivarAsync();               break;
                case "Volver":                      return;
            }
        }
    }

    private async Task<int?> SeleccionarTerminalIdAsync()
    {
        var aeropuerto = await SelectorUI.SeleccionarAeropuertoAsync(_provider);
        if (aeropuerto is null) return null;
        var terminal = await SelectorUI.SeleccionarTerminalAsync(_provider, aeropuerto.Id);
        return terminal?.Id;
    }

    private async Task ListarPorTerminalAsync()
    {
        var termId = await SeleccionarTerminalIdAsync();
        if (termId is null) return;
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetGatesByTerminalUseCase>().ExecuteAsync(termId.Value);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin puertas en esta terminal."); SpectreHelper.EsperarTecla(); return; }
            var tabla = SpectreHelper.CrearTabla("ID", "Código", "Activa");
            foreach (var g in lista)
                SpectreHelper.AgregarFila(tabla, g.Id.ToString(), g.Codigo.Valor, g.Activa.Valor ? "Sí" : "No");
            SpectreHelper.MostrarTabla(tabla); SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarActivasPorTerminalAsync()
    {
        var termId = await SeleccionarTerminalIdAsync();
        if (termId is null) return;
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetActiveGatesByTerminalUseCase>().ExecuteAsync(termId.Value);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin puertas activas."); SpectreHelper.EsperarTecla(); return; }
            var tabla = SpectreHelper.CrearTabla("ID", "Código");
            foreach (var g in lista) SpectreHelper.AgregarFila(tabla, g.Id.ToString(), g.Codigo.Valor);
            SpectreHelper.MostrarTabla(tabla); SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nueva Puerta de Embarque");
        var termId = await SeleccionarTerminalIdAsync();
        if (termId is null) return;
        var codigo = SpectreHelper.PedirTexto("Código de la puerta (ej: A1, B23)");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<CreateGateUseCase>().ExecuteAsync(termId.Value, codigo);
            SpectreHelper.MostrarExito($"Puerta '{r.Codigo.Valor}' creada (ID {r.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        var termId = await SeleccionarTerminalIdAsync();
        if (termId is null) return;
        var puerta = await SelectorUI.SeleccionarPuertaAsync(_provider, termId.Value);
        if (puerta is null) return;

        SpectreHelper.MostrarSubtitulo($"Editando puerta: {puerta.Codigo.Valor}");
        var codigo = SpectreHelper.PedirTexto($"Nuevo código  (actual: {puerta.Codigo.Valor})");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider.GetRequiredService<UpdateGateUseCase>().ExecuteAsync(puerta.Id, codigo);
            SpectreHelper.MostrarExito($"Puerta '{r.Codigo.Valor}' actualizada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ActivarAsync()
    {
        var termId = await SeleccionarTerminalIdAsync();
        if (termId is null) return;
        var puerta = await SelectorUI.SeleccionarPuertaAsync(_provider, termId.Value);
        if (puerta is null) return;
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<ActivateGateUseCase>().ExecuteAsync(puerta.Id);
            SpectreHelper.MostrarExito($"Puerta '{puerta.Codigo.Valor}' activada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task DesactivarAsync()
    {
        var termId = await SeleccionarTerminalIdAsync();
        if (termId is null) return;
        var puerta = await SelectorUI.SeleccionarPuertaAsync(_provider, termId.Value);
        if (puerta is null) return;
        if (!SpectreHelper.Confirmar($"¿Desactivar puerta '{puerta.Codigo.Valor}'?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeactivateGateUseCase>().ExecuteAsync(puerta.Id);
            SpectreHelper.MostrarExito($"Puerta '{puerta.Codigo.Valor}' desactivada.");
        });
        SpectreHelper.EsperarTecla();
    }
}
