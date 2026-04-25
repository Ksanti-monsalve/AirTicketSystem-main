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

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione un módulo",
                [
                    "Pagos",
                    "Facturación",
                    "Usuarios y roles",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Pagos":
                    await new PaymentMenu(_provider).MostrarAsync();
                    break;
                case "Facturación":
                    await new InvoiceMenu(_provider).MostrarAsync();
                    break;
                case "Usuarios y roles":
                    await new UserAdminMenu(_provider).MostrarAsync();
                    break;
                case "Volver":
                    return;
            }
        }
    }
}
