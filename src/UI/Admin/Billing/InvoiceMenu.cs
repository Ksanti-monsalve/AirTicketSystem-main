// src/UI/Admin/Billing/InvoiceMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.invoice.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Billing;

public sealed class InvoiceMenu
{
    private readonly IServiceProvider _provider;

    public InvoiceMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Facturación");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Consultar factura por reserva",
                    "Consultar factura por número",
                    "Generar factura",
                    "Actualizar dirección de facturación",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Consultar factura por reserva":        await ConsultarPorReservaAsync();  break;
                case "Consultar factura por número":         await ConsultarPorNumeroAsync();   break;
                case "Generar factura":                      await GenerarAsync();              break;
                case "Actualizar dirección de facturación":  await ActualizarDireccionAsync();  break;
                case "Volver":                               return;
            }
        }
    }

    private async Task ConsultarPorReservaAsync()
    {
        var reservaId = SpectreHelper.PedirEntero("ID de la reserva");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var inv = await scope.ServiceProvider
                .GetRequiredService<GetInvoiceByBookingUseCase>().ExecuteAsync(reservaId);
            MostrarDetalleFactura(inv);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ConsultarPorNumeroAsync()
    {
        var numero = SpectreHelper.PedirTexto("Número de factura");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var inv = await scope.ServiceProvider
                .GetRequiredService<GetInvoiceByNumeroUseCase>().ExecuteAsync(numero);
            MostrarDetalleFactura(inv);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task GenerarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Generar Factura");
        var reservaId    = SpectreHelper.PedirEntero("ID de la reserva (debe estar CONFIRMADA)");
        var direccionId  = SpectreHelper.PedirEntero("ID de la dirección de facturación");
        var subtotal     = SpectreHelper.PedirDecimal("Subtotal");
        var porcImpuesto = SpectreHelper.PedirDecimal("Porcentaje de impuesto (0 si exento)");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var inv = await scope.ServiceProvider.GetRequiredService<GenerateInvoiceUseCase>()
                .ExecuteAsync(reservaId, direccionId, subtotal, porcImpuesto);
            SpectreHelper.MostrarExito($"Factura '{inv.NumeroFactura.Valor}' generada (ID {inv.Id}). Total: {inv.Total.Valor:C2}.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ActualizarDireccionAsync()
    {
        var facturaId   = SpectreHelper.PedirEntero("ID de la factura");
        var direccionId = SpectreHelper.PedirEntero("ID de la nueva dirección de facturación");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var inv = await scope.ServiceProvider.GetRequiredService<UpdateInvoiceAddressUseCase>()
                .ExecuteAsync(facturaId, direccionId);
            SpectreHelper.MostrarExito($"Dirección de facturación de la factura {inv.Id} actualizada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private static void MostrarDetalleFactura(
        AirTicketSystem.modules.invoice.Domain.aggregate.Invoice inv)
    {
        var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
        SpectreHelper.AgregarFila(tabla, "ID",             inv.Id.ToString());
        SpectreHelper.AgregarFila(tabla, "Número",         inv.NumeroFactura.Valor);
        SpectreHelper.AgregarFila(tabla, "ReservaID",      inv.ReservaId.ToString());
        SpectreHelper.AgregarFila(tabla, "DirecciónID",    inv.DireccionFacturacionId.ToString());
        SpectreHelper.AgregarFila(tabla, "Fecha emisión",  inv.FechaEmision.Valor.ToString("yyyy-MM-dd HH:mm"));
        SpectreHelper.AgregarFila(tabla, "Subtotal",       inv.Subtotal.Valor.ToString("C2"));
        SpectreHelper.AgregarFila(tabla, "Impuestos",      inv.Impuestos.Valor.ToString("C2"));
        SpectreHelper.AgregarFila(tabla, "Total",          inv.Total.Valor.ToString("C2"));
        SpectreHelper.MostrarTabla(tabla);
    }
}
