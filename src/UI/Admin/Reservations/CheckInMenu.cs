// src/UI/Admin/Reservations/CheckInMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.checkin.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Reservations;

public sealed class CheckInMenu
{
    private readonly IServiceProvider _provider;

    public CheckInMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Check-in");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Consultar check-in",
                    "Check-in virtual (online)",
                    "Check-in presencial (mostrador)",
                    "Completar check-in",
                    "Cancelar check-in",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Consultar check-in":             await ConsultarAsync();           break;
                case "Check-in virtual (online)":      await CrearVirtualAsync();        break;
                case "Check-in presencial (mostrador)": await CrearPresencialAsync();    break;
                case "Completar check-in":             await CompletarAsync();           break;
                case "Cancelar check-in":              await CancelarAsync();            break;
                case "Volver":                         return;
            }
        }
    }

    private async Task ConsultarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del check-in");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var c = await scope.ServiceProvider.GetRequiredService<GetCheckInByIdUseCase>()
                .ExecuteAsync(id);
            if (c is null) { SpectreHelper.MostrarInfo("Check-in no encontrado."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
            SpectreHelper.AgregarFila(tabla, "ID",               c.Id.ToString());
            SpectreHelper.AgregarFila(tabla, "PasajeroReservaID", c.PasajeroReservaId.ToString());
            SpectreHelper.AgregarFila(tabla, "Tipo",             c.Tipo.Valor);
            SpectreHelper.AgregarFila(tabla, "Estado",           c.Estado.Valor);
            SpectreHelper.AgregarFila(tabla, "TrabajadorID",     c.TrabajadorId?.ToString() ?? "-");
            SpectreHelper.AgregarFila(tabla, "Fecha",            c.FechaCheckin.Valor.ToString("yyyy-MM-dd HH:mm"));
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearVirtualAsync()
    {
        SpectreHelper.MostrarSubtitulo("Check-in Virtual (Online)");
        var pasajeroReservaId = SpectreHelper.PedirEntero("ID del pasajero-reserva");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var c = await scope.ServiceProvider.GetRequiredService<CreateVirtualCheckInUseCase>()
                .ExecuteAsync(pasajeroReservaId);
            SpectreHelper.MostrarExito($"Check-in virtual creado (ID {c.Id}). Estado: {c.Estado.Valor}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task CrearPresencialAsync()
    {
        SpectreHelper.MostrarSubtitulo("Check-in Presencial (Mostrador)");
        var pasajeroReservaId = SpectreHelper.PedirEntero("ID del pasajero-reserva");
        var trabajadorId      = SpectreHelper.PedirEntero("ID del trabajador en mostrador");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var c = await scope.ServiceProvider.GetRequiredService<CreatePresentialCheckInUseCase>()
                .ExecuteAsync(pasajeroReservaId, trabajadorId);
            SpectreHelper.MostrarExito($"Check-in presencial creado (ID {c.Id}). Trabajador: {c.TrabajadorId}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task CompletarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del check-in a completar");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var c = await scope.ServiceProvider.GetRequiredService<CompleteCheckInUseCase>()
                .ExecuteAsync(id);
            SpectreHelper.MostrarExito($"Check-in {c.Id} completado. {(c.PermiteGenerarPaseAbordar ? "Puede generarse pase de abordaje." : "")}");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task CancelarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del check-in a cancelar");
        if (!SpectreHelper.Confirmar("¿Confirma cancelar el check-in?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var c = await scope.ServiceProvider.GetRequiredService<CancelCheckInUseCase>()
                .ExecuteAsync(id);
            SpectreHelper.MostrarExito($"Check-in {c.Id} cancelado.");
        });
        SpectreHelper.EsperarTecla();
    }
}
