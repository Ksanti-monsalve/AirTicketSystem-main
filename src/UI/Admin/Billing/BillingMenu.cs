// src/UI/Admin/Billing/BillingMenu.cs
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;

namespace AirTicketSystem.UI.Admin.Billing;

public sealed class BillingMenu
{
    private readonly IServiceProvider _provider;

    public BillingMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Pagos, Facturación y Usuarios");

            // Orden: roles y acceso; luego cobro y documento fiscal.
            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione un módulo",
                [
                    "Usuarios y roles",
                    "Pagos",
                    "Facturación",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Usuarios y roles":
                    await new UserAdminMenu(_provider).MostrarAsync();
                    break;
                case "Pagos":
                    await new PaymentMenu(_provider).MostrarAsync();
                    break;
                case "Facturación":
                    await new InvoiceMenu(_provider).MostrarAsync();
                    break;
                case "Volver":
                    return;
            }
        }
    }
}
