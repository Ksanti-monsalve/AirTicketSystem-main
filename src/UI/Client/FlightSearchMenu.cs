// src/UI/Client/FlightSearchMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.flight.Application.UseCases;
using AirTicketSystem.modules.airport.Application.UseCases;
using AirTicketSystem.modules.seatavailability.Application.UseCases;
using AirTicketSystem.modules.booking.Application.UseCases;
using AirTicketSystem.modules.bookingpassenger.Application.UseCases;
using AirTicketSystem.modules.fare.Application.UseCases;
using AirTicketSystem.modules.payment.Application.UseCases;

namespace AirTicketSystem.UI.Client;

public sealed class FlightSearchMenu
{
    private readonly IServiceProvider _provider;
    private readonly SessionContext   _session;

    public FlightSearchMenu(IServiceProvider provider, SessionContext session)
    {
        _provider = provider;
        _session  = session;
    }

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Buscar Vuelos y Reservar");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "1.1 Buscar vuelos (origen, destino y fecha)",
                    "1.2 Ver vuelos próximos disponibles",
                    "Ver asientos disponibles de un vuelo",
                    "Crear reserva",
                    "Agregar pasajero a reserva",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "1.1 Buscar vuelos (origen, destino y fecha)": await BuscarVuelosAsync();        break;
                case "1.2 Ver vuelos próximos disponibles":          await VuelosProximosAsync();      break;
                case "Ver asientos disponibles de un vuelo":         await VerAsientosAsync();         break;
                case "Crear reserva":                                await CrearReservaAsync();        break;
                case "Agregar pasajero a reserva":                   await AgregarPasajeroAsync();     break;
                case "Volver":                                       return;
            }
        }
    }

    private async Task VuelosProximosAsync()
    {
        SpectreHelper.MostrarSubtitulo("1.2 — Vuelos Próximos Disponibles");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var vuelos = await scope.ServiceProvider.GetRequiredService<GetScheduledFlightsUseCase>().ExecuteAsync();
            var availUc = scope.ServiceProvider.GetRequiredService<GetAvailableSeatsByFlightUseCase>();

            // LINQ: próximos 7 días, con asientos disponibles
            var hoy    = DateTime.UtcNow;
            var limite = hoy.AddDays(7);

            var proximos = vuelos
                .Where(v => v.FechaSalida.Valor >= hoy && v.FechaSalida.Valor <= limite)
                .OrderBy(v => v.FechaSalida.Valor)
                .ToList();

            if (proximos.Count == 0) { SpectreHelper.MostrarInfo("Sin vuelos disponibles en los próximos 7 días."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("VueloID", "Número", "RutaID", "Salida", "Llegada", "Estado");
            foreach (var v in proximos)
            {
                var disponibles = await availUc.ExecuteAsync(v.Id);
                if (disponibles.Count > 0)
                    SpectreHelper.AgregarFila(tabla,
                        v.Id.ToString(), v.NumeroVuelo.Valor, v.RutaId.ToString(),
                        v.FechaSalida.Valor.ToString("yyyy-MM-dd HH:mm"),
                        v.FechaLlegadaEstimada.Valor.ToString("yyyy-MM-dd HH:mm"),
                        $"{v.Estado.Valor} ({disponibles.Count} asientos)");
            }
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task BuscarVuelosAsync()
    {
        SpectreHelper.MostrarSubtitulo("Buscar Vuelos — Seleccione origen");
        var origen = await SelectorUI.SeleccionarAeropuertoAsync(_provider);
        if (origen is null) return;

        SpectreHelper.MostrarSubtitulo("Seleccione destino");
        var destino = await SelectorUI.SeleccionarAeropuertoAsync(_provider);
        if (destino is null) return;

        var origenId  = origen.Id;
        var destinoId = destino.Id;
        var fechaStr  = SpectreHelper.PedirTexto($"Fecha de viaje  {origen.CodigoIata.Valor}→{destino.CodigoIata.Valor}  (yyyy-MM-dd)");

        if (!DateTime.TryParse(fechaStr, out var fecha))
        {
            SpectreHelper.MostrarError("Fecha inválida."); SpectreHelper.EsperarTecla(); return;
        }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var vuelos = await scope.ServiceProvider.GetRequiredService<SearchFlightsUseCase>()
                .ExecuteAsync(origenId, destinoId, fecha);

            var tabla = SpectreHelper.CrearTabla("ID", "Número", "Salida", "Llegada Est.", "Estado");
            foreach (var v in vuelos)
                SpectreHelper.AgregarFila(tabla,
                    v.Id.ToString(), v.NumeroVuelo.Valor,
                    v.FechaSalida.Valor.ToString("yyyy-MM-dd HH:mm"),
                    v.FechaLlegadaEstimada.Valor.ToString("yyyy-MM-dd HH:mm"),
                    v.Estado.Valor);
            SpectreHelper.MostrarTabla(tabla);

            // Mostrar tarifas activas para cada vuelo encontrado
            SpectreHelper.MostrarSubtitulo("Tarifas disponibles");
            foreach (var v in vuelos)
            {
                await using var scopeT = _provider.CreateAsyncScope();
                var tarifas = await scopeT.ServiceProvider
                    .GetRequiredService<GetActiveFaresByRouteUseCase>().ExecuteAsync(v.RutaId);
                if (tarifas.Count == 0) continue;
                SpectreHelper.MostrarInfo($"Vuelo {v.NumeroVuelo.Valor} (ID {v.Id}):");
                var tablaT = SpectreHelper.CrearTabla("TarifaID", "Nombre", "ClaseID", "Total", "Cambios", "Reembolso");
                foreach (var t in tarifas)
                    SpectreHelper.AgregarFila(tablaT,
                        t.Id.ToString(), t.Nombre.Valor,
                        t.ClaseServicioId.ToString(),
                        t.PrecioTotal.Valor.ToString("C2"),
                        t.PermiteCambios.Valor ? "Sí" : "No",
                        t.PermiteReembolso.Valor ? "Sí" : "No");
                SpectreHelper.MostrarTabla(tablaT);
            }

            SpectreHelper.EsperarTecla();
        });
    }

    private async Task VerAsientosAsync()
    {
        SpectreHelper.MostrarSubtitulo("Asientos disponibles — Seleccione el vuelo");
        var vuelo = await SelectorUI.SeleccionarVueloProgramadoAsync(_provider);
        if (vuelo is null) return;

        var vueloId = vuelo.Id;
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var asientos = await scope.ServiceProvider
                .GetRequiredService<GetAvailableSeatsByFlightUseCase>().ExecuteAsync(vueloId);

            if (asientos.Count == 0)
            {
                SpectreHelper.MostrarInfo("No hay asientos disponibles para ese vuelo.");
                SpectreHelper.EsperarTecla(); return;
            }

            var tabla = SpectreHelper.CrearTabla("AsientoID", "Estado");
            foreach (var s in asientos)
                SpectreHelper.AgregarFila(tabla, s.AsientoId.ToString(), s.Estado.Valor);
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearReservaAsync()
    {
        SpectreHelper.MostrarSubtitulo("Nueva Reserva — Seleccione el vuelo");
        var vuelo = await SelectorUI.SeleccionarVueloProgramadoAsync(_provider);
        if (vuelo is null) return;

        SpectreHelper.MostrarSubtitulo($"Vuelo {vuelo.NumeroVuelo.Valor} — Seleccione tarifa");
        var tarifa = await SelectorUI.SeleccionarTarifaAsync(_provider, vuelo.RutaId);
        if (tarifa is null) return;

        SpectreHelper.MostrarSubtitulo("Pago — Seleccione método de pago");
        var metodoPago = await SelectorUI.SeleccionarMetodoPagoAsync(_provider);
        if (metodoPago is null) return;

        var obs    = SpectreHelper.PedirTexto("Observaciones (opcional)", obligatorio: false);
        string? obsOpc = string.IsNullOrWhiteSpace(obs) ? null : obs;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scopeC = _provider.CreateAsyncScope();
            var clienteId = await ObtenerClienteIdAsync(scopeC.ServiceProvider);

            await using var scope = _provider.CreateAsyncScope();
            var b = await scope.ServiceProvider.GetRequiredService<CreateBookingUseCase>()
                .ExecuteAsync(clienteId, vuelo.Id, tarifa.Id, tarifa.PrecioTotal.Valor, obsOpc);

            // En la creación de la reserva se deben registrar los pasajeros
            var addPassengerUc = scope.ServiceProvider.GetRequiredService<AddPassengerUseCase>();
            var availUc        = scope.ServiceProvider.GetRequiredService<GetAvailableSeatsByFlightUseCase>();

            var agregados = 0;
            while (true)
            {
                SpectreHelper.MostrarSubtitulo("Pasajero — Seleccione persona");
                var persona = await SelectorUI.SeleccionarPersonaAsync(_provider);
                if (persona is null)
                {
                    if (agregados == 0)
                        throw new InvalidOperationException(
                            "Debe agregar al menos un pasajero para completar la reserva.");
                    break;
                }

                var tipoOpc = SpectreHelper.SeleccionarOpcionTexto("Tipo de pasajero", ["ADULTO", "MENOR", "INFANTE"]);

                int? asientoId = null;
                if (SpectreHelper.Confirmar("¿Desea seleccionar asiento ahora?"))
                {
                    var disponibles = await availUc.ExecuteAsync(vuelo.Id);
                    if (disponibles.Count == 0)
                    {
                        SpectreHelper.MostrarAdvertencia("No hay asientos disponibles para este vuelo.");
                    }
                    else
                    {
                        var seleccionado = SpectreHelper.SeleccionarOpcion(
                            "Seleccione el asiento",
                            disponibles,
                            s => $"  AsientoID:{s.AsientoId}  Estado:{s.Estado.Valor}");
                        asientoId = seleccionado.AsientoId;
                    }
                }

                _ = await addPassengerUc.ExecuteAsync(b.Id, persona.Id, tipoOpc, asientoId);
                agregados++;

                if (!SpectreHelper.Confirmar("¿Desea agregar otro pasajero?"))
                    break;
            }

            // Portal cliente: pago inmediato y confirmación de reserva
            var pago = await scope.ServiceProvider.GetRequiredService<CreatePaymentUseCase>()
                .ExecuteAsync(b.Id, metodoPago.Id, b.ValorTotal.Valor);

            _ = await scope.ServiceProvider.GetRequiredService<ApprovePaymentUseCase>()
                .ExecuteAsync(pago.Id, $"CLIENT-{Guid.NewGuid():N}");

            _ = await scope.ServiceProvider.GetRequiredService<ConfirmBookingUseCase>()
                .ExecuteAsync(b.Id, _session.CurrentUserId > 0 ? _session.CurrentUserId : null);

            SpectreHelper.MostrarExito(
                $"Reserva creada exitosamente.\n" +
                $"  Código : {b.CodigoReserva.Valor}\n" +
                $"  Estado : {b.Estado.Valor}\n" +
                $"  Expira : {b.FechaExpiracion.Valor:yyyy-MM-dd HH:mm}\n" +
                $"  Total  : {b.ValorTotal.Valor:C2}");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task AgregarPasajeroAsync()
    {
        SpectreHelper.MostrarSubtitulo("Agregar Pasajero — Seleccione una reserva");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scopeC = _provider.CreateAsyncScope();
            var clienteId = await ObtenerClienteIdAsync(scopeC.ServiceProvider);
            var reserva = await SelectorUI.SeleccionarReservaAsync(_provider, clienteId);
            if (reserva is null) return;

            var persona  = await SelectorUI.SeleccionarPersonaAsync(_provider);
            if (persona is null) return;

            var tipoOpc  = SpectreHelper.SeleccionarOpcionTexto("Tipo de pasajero", ["ADULTO", "MENOR", "INFANTE"]);
            var asientoStr = SpectreHelper.PedirTexto("ID del asiento (opcional, Enter para omitir)");
            int? asientoId = int.TryParse(asientoStr, out var av) && av > 0 ? av : null;

            await using var scope = _provider.CreateAsyncScope();
            var p = await scope.ServiceProvider.GetRequiredService<AddPassengerUseCase>()
                .ExecuteAsync(reserva.Id, persona.Id, tipoOpc, asientoId);
            SpectreHelper.MostrarExito($"Pasajero agregado (ID {p.Id}). Tipo: {p.TipoPasajero.Valor}.");
        });

        SpectreHelper.EsperarTecla();
    }

    // Resuelve el clienteId buscando por usuarioId en la BD
    private async Task<int> ObtenerClienteIdAsync(IServiceProvider sp)
    {
        var clientes = await sp.GetRequiredService<AirTicketSystem.modules.client.Application.UseCases.GetAllClientsUseCase>()
            .ExecuteAsync();
        var cliente = clientes.FirstOrDefault(c => c.UsuarioId == _session.CurrentUserId)
            ?? throw new InvalidOperationException(
                "No se encontró el perfil de cliente asociado a su usuario. Contacte al administrador.");
        return cliente.Id;
    }

}
