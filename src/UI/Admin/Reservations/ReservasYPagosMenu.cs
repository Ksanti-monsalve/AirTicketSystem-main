// src/UI/Admin/Reservations/ReservasYPagosMenu.cs
using AirTicketSystem.shared.UI;
using AirTicketSystem.UI.Admin.Billing;

namespace AirTicketSystem.UI.Admin.Reservations;

public sealed class ReservasYPagosMenu
{
    private readonly IServiceProvider _provider;
    private readonly SessionContext   _session;

    public ReservasYPagosMenu(IServiceProvider provider, SessionContext session)
    {
        _provider = provider;
        _session  = session;
    }

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Gestión de Reservas y Pagos");

            // Orden: reserva → cobro y documentos → embarque → incidencia de cierre contable.
            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione un módulo",
                [
                    "6.1 Reservas",
                    "6.2 Pagos",
                    "6.3 Tiquetes",
                    "6.4 Check-in",
                    "6.5 Equipaje",
                    "6.6 Cargos adicionales",
                    "6.7 Facturas",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "6.1 Reservas":           await new BookingMenu(_provider, _session).MostrarAsync();  break;
                case "6.2 Pagos":              await new PaymentMenu(_provider).MostrarAsync();            break;
                case "6.3 Tiquetes":           await new TicketAdminMenu(_provider).MostrarAsync();        break;
                case "6.4 Check-in":           await new CheckInMenu(_provider).MostrarAsync();            break;
                case "6.5 Equipaje":           await new LuggageAdminMenu(_provider).MostrarAsync();       break;
                case "6.6 Cargos adicionales": await new AdditionalChargeMenu(_provider).MostrarAsync();  break;
                case "6.7 Facturas":           await new InvoiceMenu(_provider).MostrarAsync();            break;
                case "Volver":                return;
            }
        }
    }
}
