// src/UI/Admin/Reservations/PassengerMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.bookingpassenger.Application.UseCases;
using AirTicketSystem.modules.booking.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Reservations;

public sealed class PassengerMenu
{
    private readonly IServiceProvider _provider;

    public PassengerMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Pasajeros de Reserva");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Listar pasajeros de reserva",
                    "Agregar pasajero",
                    "Asignar asiento",
                    "Cambiar asiento",
                    "Liberar asiento",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Listar pasajeros de reserva": await ListarPorReservaAsync(); break;
                case "Agregar pasajero":            await AgregarAsync();          break;
                case "Asignar asiento":             await AsignarAsientoAsync();   break;
                case "Cambiar asiento":             await CambiarAsientoAsync();   break;
                case "Liberar asiento":             await LiberarAsientoAsync();   break;
                case "Volver":                      return;
            }
        }
    }

    private async Task ListarPorReservaAsync()
    {
        var reservaId = SpectreHelper.PedirEntero("ID de la reserva");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetPassengersByBookingUseCase>()
                .ExecuteAsync(reservaId);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin pasajeros en esa reserva."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "PersonaID", "Tipo", "AsientoID");
            foreach (var p in lista)
                SpectreHelper.AgregarFila(tabla,
                    p.Id.ToString(), p.PersonaId.ToString(),
                    p.TipoPasajero.Valor,
                    p.AsientoId?.ToString() ?? "Sin asignar");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task AgregarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Agregar Pasajero a Reserva");

        var reservaId  = SpectreHelper.PedirEntero("ID de la reserva");

        await MostrarReservaAsync(reservaId);

        var personaId  = SpectreHelper.PedirEntero("ID de la persona");
        var tipoOpc    = SpectreHelper.SeleccionarOpcionTexto("Tipo de pasajero", ["ADULTO", "MENOR", "INFANTE"]);
        var asientoStr = SpectreHelper.PedirTexto("ID del asiento (opcional, Enter para omitir)");
        int? asientoId = int.TryParse(asientoStr, out var av) && av > 0 ? av : null;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var p = await scope.ServiceProvider.GetRequiredService<AddPassengerUseCase>()
                .ExecuteAsync(reservaId, personaId, tipoOpc, asientoId);
            SpectreHelper.MostrarExito($"Pasajero agregado (ID {p.Id}). Tipo: {p.TipoPasajero.Valor}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task AsignarAsientoAsync()
    {
        var pasajeroReservaId = SpectreHelper.PedirEntero("ID del pasajero-reserva");
        var asientoId         = SpectreHelper.PedirEntero("ID del asiento a asignar");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var p = await scope.ServiceProvider.GetRequiredService<AssignSeatUseCase>()
                .ExecuteAsync(pasajeroReservaId, asientoId);
            SpectreHelper.MostrarExito($"Asiento {p.AsientoId} asignado al pasajero {p.Id}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task CambiarAsientoAsync()
    {
        var pasajeroReservaId = SpectreHelper.PedirEntero("ID del pasajero-reserva");
        var nuevoAsientoId    = SpectreHelper.PedirEntero("ID del nuevo asiento");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var p = await scope.ServiceProvider.GetRequiredService<ChangeSeatUseCase>()
                .ExecuteAsync(pasajeroReservaId, nuevoAsientoId);
            SpectreHelper.MostrarExito($"Asiento cambiado al {p.AsientoId}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task LiberarAsientoAsync()
    {
        var pasajeroReservaId = SpectreHelper.PedirEntero("ID del pasajero-reserva");
        if (!SpectreHelper.Confirmar("¿Confirma liberar el asiento?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var p = await scope.ServiceProvider.GetRequiredService<ReleaseSeatUseCase>()
                .ExecuteAsync(pasajeroReservaId);
            SpectreHelper.MostrarExito($"Asiento liberado del pasajero {p.Id}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task MostrarReservaAsync(int reservaId)
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var b = await scope.ServiceProvider.GetRequiredService<GetBookingByIdUseCase>()
                .ExecuteAsync(reservaId);
            if (b is null) return;
            SpectreHelper.MostrarInfo($"Reserva [{b.CodigoReserva.Valor}] — Vuelo {b.VueloId} — Estado: {b.Estado.Valor} — Total: {b.ValorTotal.Valor:C2}");
        });
    }
}
