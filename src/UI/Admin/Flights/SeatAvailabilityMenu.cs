// src/UI/Admin/Flights/SeatAvailabilityMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.seatavailability.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Flights;

public sealed class SeatAvailabilityMenu
{
    private readonly IServiceProvider _provider;

    public SeatAvailabilityMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Disponibilidad de Asientos");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Ver asientos disponibles por vuelo",
                    "Reservar asiento",
                    "Liberar asiento",
                    "Bloquear asiento",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Ver asientos disponibles por vuelo": await VerDisponiblesAsync(); break;
                case "Reservar asiento":                   await ReservarAsync();       break;
                case "Liberar asiento":                    await LiberarAsync();        break;
                case "Bloquear asiento":                   await BloquearAsync();       break;
                case "Volver":                             return;
            }
        }
    }

    private async Task VerDisponiblesAsync()
    {
        var vueloId = SpectreHelper.PedirEntero("ID del vuelo");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAvailableSeatsByFlightUseCase>().ExecuteAsync(vueloId);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin asientos disponibles en este vuelo."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "AsientoID", "Estado");
            foreach (var s in lista)
                SpectreHelper.AgregarFila(tabla, s.Id.ToString(), s.AsientoId.ToString(), s.Estado.Valor);
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.MostrarInfo($"Total disponibles: {lista.Count}");
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ReservarAsync()
    {
        var vueloId   = SpectreHelper.PedirEntero("ID del vuelo");
        var asientoId = SpectreHelper.PedirEntero("ID del asiento");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<ReserveSeatUseCase>().ExecuteAsync(vueloId, asientoId);
            SpectreHelper.MostrarExito("Asiento reservado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task LiberarAsync()
    {
        var vueloId   = SpectreHelper.PedirEntero("ID del vuelo");
        var asientoId = SpectreHelper.PedirEntero("ID del asiento");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<AirTicketSystem.modules.seatavailability.Application.UseCases.ReleaseSeatUseCase>()
                .ExecuteAsync(vueloId, asientoId);
            SpectreHelper.MostrarExito("Asiento liberado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task BloquearAsync()
    {
        var vueloId   = SpectreHelper.PedirEntero("ID del vuelo");
        var asientoId = SpectreHelper.PedirEntero("ID del asiento");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<BlockSeatUseCase>().ExecuteAsync(vueloId, asientoId);
            SpectreHelper.MostrarExito("Asiento bloqueado.");
        });
        SpectreHelper.EsperarTecla();
    }
}
