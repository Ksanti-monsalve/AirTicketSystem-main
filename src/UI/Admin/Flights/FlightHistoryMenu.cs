// src/UI/Admin/Flights/FlightHistoryMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.flighthistory.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Flights;

public sealed class FlightHistoryMenu
{
    private readonly IServiceProvider _provider;

    public FlightHistoryMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Historial de Vuelos");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Ver historial de vuelo",
                    "Ver último cambio de vuelo",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Ver historial de vuelo":       await VerHistorialAsync();     break;
                case "Ver último cambio de vuelo":   await VerUltimoCambioAsync();  break;
                case "Volver":                       return;
            }
        }
    }

    private async Task VerHistorialAsync()
    {
        var vueloId = SpectreHelper.PedirEntero("ID del vuelo");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetFlightHistoryUseCase>().ExecuteAsync(vueloId);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin historial para este vuelo."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Estado anterior", "Estado nuevo", "Fecha cambio", "UsuarioID", "Motivo");
            foreach (var h in lista.OrderByDescending(h => h.FechaCambio))
                SpectreHelper.AgregarFila(tabla,
                    h.Id.ToString(),
                    h.EstadoAnterior.Valor, h.EstadoNuevo.Valor,
                    h.FechaCambio.ToString("yyyy-MM-dd HH:mm"),
                    h.UsuarioId?.ToString() ?? "-",
                    h.Motivo?.Valor ?? "-");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task VerUltimoCambioAsync()
    {
        var vueloId = SpectreHelper.PedirEntero("ID del vuelo");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var h = await scope.ServiceProvider.GetRequiredService<GetLastFlightChangeUseCase>().ExecuteAsync(vueloId);
            if (h is null) { SpectreHelper.MostrarInfo("Sin cambios registrados."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
            SpectreHelper.AgregarFila(tabla, "Estado anterior", h.EstadoAnterior.Valor);
            SpectreHelper.AgregarFila(tabla, "Estado nuevo",    h.EstadoNuevo.Valor);
            SpectreHelper.AgregarFila(tabla, "Fecha cambio",    h.FechaCambio.ToString("yyyy-MM-dd HH:mm"));
            SpectreHelper.AgregarFila(tabla, "UsuarioID",       h.UsuarioId?.ToString() ?? "-");
            SpectreHelper.AgregarFila(tabla, "Motivo",          h.Motivo?.Valor ?? "-");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }
}
