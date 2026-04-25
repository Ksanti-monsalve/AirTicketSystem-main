// src/UI/Admin/Reservations/BookingMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.booking.Application.UseCases;
using AirTicketSystem.modules.flight.Application.UseCases;
using AirTicketSystem.modules.fare.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Reservations;

public sealed class BookingMenu
{
    private readonly IServiceProvider _provider;
    private readonly SessionContext   _session;

    public BookingMenu(IServiceProvider provider, SessionContext session)
    {
        _provider = provider;
        _session  = session;
    }

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Reservas");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Buscar por código",
                    "Listar por cliente",
                    "Crear reserva",
                    "Confirmar reserva",
                    "Cancelar reserva",
                    "Extender expiración",
                    "Actualizar observaciones",
                    "Expirar reserva",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Buscar por código":        await BuscarPorCodigoAsync();       break;
                case "Listar por cliente":       await ListarPorClienteAsync();      break;
                case "Crear reserva":            await CrearAsync();                 break;
                case "Confirmar reserva":        await ConfirmarAsync();             break;
                case "Cancelar reserva":         await CancelarAsync();              break;
                case "Extender expiración":      await ExtenderAsync();              break;
                case "Actualizar observaciones": await ActualizarObservacionesAsync(); break;
                case "Expirar reserva":          await ExpirarAsync();               break;
                case "Volver":                   return;
            }
        }
    }

    private async Task BuscarPorCodigoAsync()
    {
        var codigo = SpectreHelper.PedirTexto("Código de la reserva");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var b = await scope.ServiceProvider.GetRequiredService<GetBookingByCodigoUseCase>()
                .ExecuteAsync(codigo);
            MostrarDetalleReserva(b);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarPorClienteAsync()
    {
        var clienteId = SpectreHelper.PedirEntero("ID del cliente");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetBookingsByClienteUseCase>()
                .ExecuteAsync(clienteId);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin reservas para ese cliente."); SpectreHelper.EsperarTecla(); return; }
            MostrarTablaReservas(lista);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nueva Reserva");

        await MostrarVuelosDisponiblesAsync();
        var vueloId = SpectreHelper.PedirEntero("ID del vuelo");

        await MostrarTarifasPorVueloAsync(vueloId);
        var tarifaId = SpectreHelper.PedirEntero("ID de la tarifa");

        var clienteId   = SpectreHelper.PedirEntero("ID del cliente");
        var valorTotal  = SpectreHelper.PedirDecimal("Valor total de la reserva");
        var obs         = SpectreHelper.PedirTexto("Observaciones (opcional)");
        string? obsOpc  = string.IsNullOrWhiteSpace(obs) ? null : obs;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var b = await scope.ServiceProvider.GetRequiredService<CreateBookingUseCase>()
                .ExecuteAsync(clienteId, vueloId, tarifaId, valorTotal, obsOpc);
            SpectreHelper.MostrarExito($"Reserva creada. Código: {b.CodigoReserva.Valor} (ID {b.Id}). Expira: {b.FechaExpiracion.Valor:yyyy-MM-dd HH:mm}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ConfirmarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la reserva a confirmar");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var b = await scope.ServiceProvider.GetRequiredService<ConfirmBookingUseCase>()
                .ExecuteAsync(id, _session.CurrentUserId > 0 ? _session.CurrentUserId : null);
            SpectreHelper.MostrarExito($"Reserva [{b.CodigoReserva.Valor}] confirmada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task CancelarAsync()
    {
        var id     = SpectreHelper.PedirEntero("ID de la reserva a cancelar");
        var motivo = SpectreHelper.PedirTexto("Motivo de cancelación");
        if (!SpectreHelper.Confirmar("¿Confirma cancelar la reserva?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var b = await scope.ServiceProvider.GetRequiredService<CancelBookingUseCase>()
                .ExecuteAsync(id, motivo, _session.CurrentUserId > 0 ? _session.CurrentUserId : null);
            SpectreHelper.MostrarExito($"Reserva [{b.CodigoReserva.Valor}] cancelada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ExtenderAsync()
    {
        var id    = SpectreHelper.PedirEntero("ID de la reserva");
        var horas = SpectreHelper.PedirEntero("Horas a extender");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var b = await scope.ServiceProvider.GetRequiredService<ExtendBookingUseCase>()
                .ExecuteAsync(id, horas);
            SpectreHelper.MostrarExito($"Expiración extendida. Nueva fecha: {b.FechaExpiracion.Valor:yyyy-MM-dd HH:mm}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ActualizarObservacionesAsync()
    {
        var id  = SpectreHelper.PedirEntero("ID de la reserva");
        var obs = SpectreHelper.PedirTexto("Nuevas observaciones (Enter para borrar)");
        string? obsOpc = string.IsNullOrWhiteSpace(obs) ? null : obs;
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var b = await scope.ServiceProvider.GetRequiredService<UpdateBookingObservationsUseCase>()
                .ExecuteAsync(id, obsOpc);
            SpectreHelper.MostrarExito($"Observaciones de reserva [{b.CodigoReserva.Valor}] actualizadas.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ExpirarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID de la reserva a expirar");
        if (!SpectreHelper.Confirmar("¿Confirma expirar la reserva manualmente?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<ExpireBookingUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Reserva expirada.");
        });
        SpectreHelper.EsperarTecla();
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static void MostrarDetalleReserva(
        AirTicketSystem.modules.booking.Domain.aggregate.Booking b)
    {
        var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
        SpectreHelper.AgregarFila(tabla, "ID",           b.Id.ToString());
        SpectreHelper.AgregarFila(tabla, "Código",       b.CodigoReserva.Valor);
        SpectreHelper.AgregarFila(tabla, "ClienteID",    b.ClienteId.ToString());
        SpectreHelper.AgregarFila(tabla, "VueloID",      b.VueloId.ToString());
        SpectreHelper.AgregarFila(tabla, "TarifaID",     b.TarifaId.ToString());
        SpectreHelper.AgregarFila(tabla, "Estado",       b.Estado.Valor);
        SpectreHelper.AgregarFila(tabla, "Valor total",  b.ValorTotal.Valor.ToString("C2"));
        SpectreHelper.AgregarFila(tabla, "Fecha reserva", b.FechaReserva.Valor.ToString("yyyy-MM-dd HH:mm"));
        SpectreHelper.AgregarFila(tabla, "Expira",       b.FechaExpiracion.Valor.ToString("yyyy-MM-dd HH:mm"));
        SpectreHelper.AgregarFila(tabla, "Observaciones", b.Observaciones?.Valor ?? "-");
        SpectreHelper.MostrarTabla(tabla);
    }

    private static void MostrarTablaReservas(
        IEnumerable<AirTicketSystem.modules.booking.Domain.aggregate.Booking> lista)
    {
        var tabla = SpectreHelper.CrearTabla("ID", "Código", "VueloID", "TarifaID", "Estado", "Total", "Expira");
        foreach (var b in lista)
            SpectreHelper.AgregarFila(tabla,
                b.Id.ToString(), b.CodigoReserva.Valor,
                b.VueloId.ToString(), b.TarifaId.ToString(),
                b.Estado.Valor,
                b.ValorTotal.Valor.ToString("C2"),
                b.FechaExpiracion.Valor.ToString("yyyy-MM-dd HH:mm"));
        SpectreHelper.MostrarTabla(tabla);
    }

    private async Task MostrarVuelosDisponiblesAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetScheduledFlightsUseCase>().ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "Número", "RutaID", "Salida", "Estado");
            foreach (var f in lista)
                SpectreHelper.AgregarFila(tabla,
                    f.Id.ToString(), f.NumeroVuelo.Valor,
                    f.RutaId.ToString(),
                    f.FechaSalida.Valor.ToString("yyyy-MM-dd HH:mm"),
                    f.Estado.Valor);
            SpectreHelper.MostrarTabla(tabla);
        });
    }

    private async Task MostrarTarifasPorVueloAsync(int vueloId)
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scopeV = _provider.CreateAsyncScope();
            var vuelo = await scopeV.ServiceProvider.GetRequiredService<GetFlightByIdUseCase>()
                .ExecuteAsync(vueloId);
            if (vuelo is null) return;

            await using var scopeT = _provider.CreateAsyncScope();
            var tarifas = await scopeT.ServiceProvider.GetRequiredService<GetActiveFaresByRouteUseCase>()
                .ExecuteAsync(vuelo.RutaId);
            if (tarifas.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "Nombre", "ClaseID", "Total");
            foreach (var t in tarifas)
                SpectreHelper.AgregarFila(tabla,
                    t.Id.ToString(), t.Nombre.Valor,
                    t.ClaseServicioId.ToString(),
                    t.PrecioTotal.Valor.ToString("C2"));
            SpectreHelper.MostrarTabla(tabla);
        });
    }
}
