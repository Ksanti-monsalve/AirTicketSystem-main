// src/UI/Admin/Reservations/TicketAdminMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.ticket.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Reservations;

public sealed class TicketAdminMenu
{
    private readonly IServiceProvider _provider;

    public TicketAdminMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Tiquetes");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Consultar tiquete por código",
                    "Emitir tiquete",
                    "Hacer check-in en tiquete",
                    "Registrar abordaje",
                    "Anular tiquete",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Consultar tiquete por código": await ConsultarAsync();      break;
                case "Emitir tiquete":               await EmitirAsync();         break;
                case "Hacer check-in en tiquete":    await CheckInTicketAsync();  break;
                case "Registrar abordaje":           await AbordajeAsync();       break;
                case "Anular tiquete":               await AnularAsync();         break;
                case "Volver":                       return;
            }
        }
    }

    private async Task ConsultarAsync()
    {
        var codigo = SpectreHelper.PedirTexto("Código del tiquete");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var t = await scope.ServiceProvider.GetRequiredService<GetTicketByCodeUseCase>().ExecuteAsync(codigo);

            var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
            SpectreHelper.AgregarFila(tabla, "ID",                  t.Id.ToString());
            SpectreHelper.AgregarFila(tabla, "Código",              t.CodigoTiquete.Valor);
            SpectreHelper.AgregarFila(tabla, "PasajeroReservaID",   t.PasajeroReservaId.ToString());
            SpectreHelper.AgregarFila(tabla, "Estado",              t.Estado.Valor);
            SpectreHelper.AgregarFila(tabla, "Fecha emisión",       t.FechaEmision.Valor.ToString("yyyy-MM-dd HH:mm"));
            SpectreHelper.AgregarFila(tabla, "Asiento confirmado",  t.AsientoConfirmadoId?.ToString() ?? "-");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task EmitirAsync()
    {
        var pasajeroReservaId = SpectreHelper.PedirEntero("ID del pasajero-reserva");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var t = await scope.ServiceProvider.GetRequiredService<EmitTicketUseCase>().ExecuteAsync(pasajeroReservaId);
            SpectreHelper.MostrarExito($"Tiquete emitido. Código: {t.CodigoTiquete.Valor} (ID {t.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task CheckInTicketAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del tiquete");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var t = await scope.ServiceProvider.GetRequiredService<CheckInTicketUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito($"Check-in registrado en tiquete {t.CodigoTiquete.Valor}. Estado: {t.Estado.Valor}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task AbordajeAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del tiquete");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var t = await scope.ServiceProvider.GetRequiredService<BoardTicketUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito($"Abordaje registrado en tiquete {t.CodigoTiquete.Valor}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task AnularAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del tiquete a anular");
        if (!SpectreHelper.Confirmar("¿Confirma anular el tiquete?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<VoidTicketUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Tiquete anulado.");
        });
        SpectreHelper.EsperarTecla();
    }
}
