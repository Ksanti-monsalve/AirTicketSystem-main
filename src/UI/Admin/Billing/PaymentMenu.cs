// src/UI/Admin/Billing/PaymentMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.payment.Application.UseCases;
using AirTicketSystem.modules.paymentmethod.Domain.aggregate;

namespace AirTicketSystem.UI.Admin.Billing;

public sealed class PaymentMenu
{
    private readonly IServiceProvider _provider;

    public PaymentMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Pagos");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Listar pagos de reserva",
                    "Registrar pago",
                    "Aprobar pago",
                    "Rechazar pago",
                    "Reembolsar pago",
                    "Reintentar pago",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Listar pagos de reserva": await ListarPorReservaAsync(); break;
                case "Registrar pago":          await CrearAsync();            break;
                case "Aprobar pago":            await AprobarAsync();          break;
                case "Rechazar pago":           await RechazarAsync();         break;
                case "Reembolsar pago":         await ReembolsarAsync();       break;
                case "Reintentar pago":         await ReintentarAsync();       break;
                case "Volver":                  return;
            }
        }
    }

    private async Task ListarPorReservaAsync()
    {
        var reservaId = SpectreHelper.PedirEntero("ID de la reserva");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider
                .GetRequiredService<GetPaymentsByBookingUseCase>().ExecuteAsync(reservaId);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin pagos para esa reserva."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "MetodoPagoID", "Monto", "Estado", "Referencia", "FechaPago", "Vencimiento");
            foreach (var p in lista)
                SpectreHelper.AgregarFila(tabla,
                    p.Id.ToString(), p.MetodoPagoId.ToString(),
                    p.Monto.Valor.ToString("C2"),
                    p.Estado.Valor,
                    p.Referencia?.Valor ?? "-",
                    p.FechaPago?.Valor.ToString("yyyy-MM-dd HH:mm") ?? "-",
                    p.FechaVencimiento.Valor.ToString("yyyy-MM-dd HH:mm"));
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Registrar Pago");
        await MostrarMetodosPagoAsync();
        var reservaId    = SpectreHelper.PedirEntero("ID de la reserva");
        var metodoPago = await SelectorUI.SeleccionarMetodoPagoAsync(_provider);
        if (metodoPago is null) {
            SpectreHelper.EsperarTecla();
            return;
        }
        var metodoPagoId = metodoPago.Id;
        var monto        = SpectreHelper.PedirDecimal("Monto del pago");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var p = await scope.ServiceProvider.GetRequiredService<CreatePaymentUseCase>()
                .ExecuteAsync(reservaId, metodoPagoId, monto);
            SpectreHelper.MostrarExito($"Pago registrado (ID {p.Id}). Estado: {p.Estado.Valor}. Vence: {p.FechaVencimiento.Valor:yyyy-MM-dd HH:mm}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task AprobarAsync()
    {
        var id         = SpectreHelper.PedirEntero("ID del pago");
        var referencia = SpectreHelper.PedirTexto("Referencia / número de transacción");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var p = await scope.ServiceProvider.GetRequiredService<ApprovePaymentUseCase>()
                .ExecuteAsync(id, referencia);
            SpectreHelper.MostrarExito($"Pago {p.Id} aprobado. Referencia: {p.Referencia?.Valor}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task RechazarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del pago a rechazar");
        if (!SpectreHelper.Confirmar("¿Confirma rechazar el pago?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var p = await scope.ServiceProvider.GetRequiredService<RejectPaymentUseCase>()
                .ExecuteAsync(id);
            SpectreHelper.MostrarExito($"Pago {p.Id} rechazado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ReembolsarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del pago a reembolsar");
        if (!SpectreHelper.Confirmar("¿Confirma reembolsar el pago?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var p = await scope.ServiceProvider.GetRequiredService<RefundPaymentUseCase>()
                .ExecuteAsync(id);
            SpectreHelper.MostrarExito($"Pago {p.Id} reembolsado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ReintentarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Reintentar Pago");
        var id = SpectreHelper.PedirEntero("ID del pago fallido");
        await MostrarMetodosPagoAsync();
        var nuevoMetodo = await SelectorUI.SeleccionarMetodoPagoAsync(_provider);
        if (nuevoMetodo is null) {
            SpectreHelper.EsperarTecla();
            return;
        }
        var nuevoMetodoId = nuevoMetodo.Id;
        var montoStr      = SpectreHelper.PedirTexto("Nuevo monto (Enter para mantener el mismo)");
        decimal? monto    = decimal.TryParse(montoStr, out var mv) && mv > 0 ? mv : null;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var p = await scope.ServiceProvider.GetRequiredService<RetryPaymentUseCase>()
                .ExecuteAsync(id, nuevoMetodoId, monto);
            SpectreHelper.MostrarExito($"Pago {p.Id} reintentado. Estado: {p.Estado.Valor}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private static Task MostrarMetodosPagoAsync()
    {
        SpectreHelper.MostrarInfo("Métodos de pago habituales: 1=Tarjeta crédito, 2=Tarjeta débito, 3=PSE, 4=Efectivo. Consulte la BD para IDs exactos.");
        return Task.CompletedTask;
    }
}
