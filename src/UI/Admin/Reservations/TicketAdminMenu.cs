// src/UI/Admin/Reservations/TicketAdminMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.ticket.Application.UseCases;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;
using AirTicketSystem.modules.aircraftseat.Domain.Repositories;
using AirTicketSystem.modules.seatavailability.Domain.Repositories;
using AirTicketSystem.modules.serviceclass.Domain.Repositories;
using AirTicketSystem.modules.seat.Domain.Repositories;

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

            // EXAMEN: el tiquete debe mostrar el asiento (número) y la clase cuando exista.
            string asientoNumero = "-";
            string claseNombre   = "-";
            // Primero intentar por tabla literal del examen: seats.ticket_id -> seats.seat_number + flight_classes.name
            var seatExamRepo = scope.ServiceProvider.GetRequiredService<ISeatRepository>();
            var seatId = await seatExamRepo.FindSeatIdByTicketIdAsync(t.Id);
            if (seatId is int sid)
            {
                var detalleSeat = await seatExamRepo.FindDetailByIdAsync(sid);
                if (detalleSeat is not null)
                {
                    asientoNumero = detalleSeat.SeatNumber;
                    claseNombre   = detalleSeat.FlightClassName;
                }
            }
            else if (t.AsientoConfirmadoId is int aircraftSeatId)
            {
                // Fallback legado
                var seatRepo  = scope.ServiceProvider.GetRequiredService<IAircraftSeatRepository>();
                var classRepo = scope.ServiceProvider.GetRequiredService<IServiceClassRepository>();

                var seat = await seatRepo.FindByIdAsync(aircraftSeatId);
                if (seat is not null)
                {
                    asientoNumero = seat.CodigoAsiento.Valor;
                    var clase = await classRepo.FindByIdAsync(seat.ClaseServicioId);
                    claseNombre = clase?.Nombre.Valor ?? "-";
                }
            }

            var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
            SpectreHelper.AgregarFila(tabla, "ID",                  t.Id.ToString());
            SpectreHelper.AgregarFila(tabla, "Código",              t.CodigoTiquete.Valor);
            SpectreHelper.AgregarFila(tabla, "PasajeroReservaID",   t.PasajeroReservaId.ToString());
            SpectreHelper.AgregarFila(tabla, "Estado",              t.Estado.Valor);
            SpectreHelper.AgregarFila(tabla, "Fecha emisión",       t.FechaEmision.Valor.ToString("yyyy-MM-dd HH:mm"));
            SpectreHelper.AgregarFila(tabla, "Asiento",             asientoNumero);
            SpectreHelper.AgregarFila(tabla, "Clase",               claseNombre);
            SpectreHelper.AgregarFila(tabla, "AsientoConfirmadoId", t.AsientoConfirmadoId?.ToString() ?? "-");
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

            // Mostrar número de asiento y clase (si hay asiento asignado)
            var paxRepo = scope.ServiceProvider.GetRequiredService<IBookingPassengerRepository>();
            var saRepo  = scope.ServiceProvider.GetRequiredService<ISeatAvailabilityRepository>();
            var seatExamRepo = scope.ServiceProvider.GetRequiredService<ISeatRepository>();
            var pax     = await paxRepo.FindByIdAsync(pasajeroReservaId);

            // Preferir Seat table por ticket id (modelo literal)
            var seatId = await seatExamRepo.FindSeatIdByTicketIdAsync(t.Id);
            if (seatId is int sid)
            {
                var detalleSeat = await seatExamRepo.FindDetailByIdAsync(sid);
                SpectreHelper.MostrarExito(
                    $"Tiquete emitido.\n" +
                    $"  Código  : {t.CodigoTiquete.Valor}\n" +
                    $"  Asiento : {detalleSeat?.SeatNumber ?? "-"}\n" +
                    $"  Clase   : {detalleSeat?.FlightClassName ?? "-"}\n" +
                    $"  Estado  : {t.Estado.Valor}\n" +
                    $"  Emitido : {t.FechaEmision.Valor:yyyy-MM-dd HH:mm}");
                return;
            }

            if (pax?.AsientoId is int dispId)
            {
                var detalle = await saRepo.FindDetalleByDisponibilidadIdAsync(dispId);
                SpectreHelper.MostrarExito(
                    $"Tiquete emitido.\n" +
                    $"  Código  : {t.CodigoTiquete.Valor}\n" +
                    $"  Asiento : {detalle?.NumeroAsiento ?? "-"}\n" +
                    $"  Clase   : {detalle?.ClaseServicioNombre ?? "-"}\n" +
                    $"  Estado  : {t.Estado.Valor}\n" +
                    $"  Emitido : {t.FechaEmision.Valor:yyyy-MM-dd HH:mm}");
                return;
            }

            SpectreHelper.MostrarExito(
                $"Tiquete emitido.\n" +
                $"  Código  : {t.CodigoTiquete.Valor}\n" +
                $"  Estado  : {t.Estado.Valor}\n" +
                $"  Emitido : {t.FechaEmision.Valor:yyyy-MM-dd HH:mm}");
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
