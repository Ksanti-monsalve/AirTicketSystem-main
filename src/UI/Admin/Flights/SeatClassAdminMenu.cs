using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.flight.Application.UseCases;
using AirTicketSystem.modules.seat.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Flights;

/// <summary>
/// Menú solicitado por el examen para separar funciones de ADMIN
/// relacionadas con clases de vuelo y selección/estado de asientos por vuelo.
/// </summary>
public sealed class SeatClassAdminMenu
{
    private readonly IServiceProvider _provider;

    public SeatClassAdminMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("ADMIN — Selección de Asientos y Clases de Vuelo");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "1. Crear vuelo",
                    "2. Ver asientos por vuelo",
                    "3. Ver asientos ocupados",
                    "4. Ver disponibilidad por clase",
                    "5. Ver porcentaje de ocupación",
                    "6. Consultar reservas",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "1. Crear vuelo":
                    await CrearVueloAsync();
                    break;
                case "2. Ver asientos por vuelo":
                    await VerAsientosPorVueloAsync();
                    break;
                case "3. Ver asientos ocupados":
                    await VerAsientosOcupadosAsync();
                    break;
                case "4. Ver disponibilidad por clase":
                    await VerDisponibilidadPorClaseAsync();
                    break;
                case "5. Ver porcentaje de ocupación":
                    await VerPorcentajeOcupacionAsync();
                    break;
                case "6. Consultar reservas":
                    await ConsultarReservasAsync();
                    break;
                case "Volver":
                    return;
            }
        }
    }

    /// <summary>
    /// Reutiliza el caso de uso existente: al crear vuelo ya genera automáticamente la disponibilidad de asientos.
    /// </summary>
    private async Task CrearVueloAsync()
    {
        SpectreHelper.MostrarInfo("Se abrirá el módulo de creación de vuelos (incluye generación automática de asientos).");
        SpectreHelper.EsperarTecla();
        await new FlightMenu(_provider).MostrarAsync();
    }

    /// <summary>
    /// FASE 6: listado de asientos con número, clase y estado.
    /// ADMIN ve todos (DISPONIBLE/RESERVADO/OCUPADO/BLOQUEADO).
    /// </summary>
    private async Task VerAsientosPorVueloAsync()
    {
        var vueloId = SpectreHelper.PedirEntero("ID del vuelo");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var asientos = await scope.ServiceProvider
                .GetRequiredService<GetSeatDetailsByFlightUseCase>()
                .ExecuteAsync(vueloId);

            if (asientos.Count == 0)
            {
                SpectreHelper.MostrarInfo("No hay asientos registrados para este vuelo.");
                SpectreHelper.EsperarTecla();
                return;
            }

            // EXAMEN (punto 6): agrupar asientos por clase
            foreach (var grupo in asientos
                         .GroupBy(s => new { s.FlightClassId, s.FlightClassName })
                         .OrderBy(g => g.Key.FlightClassName))
            {
                SpectreHelper.MostrarSubtitulo($"Clase: {grupo.Key.FlightClassName} (ID {grupo.Key.FlightClassId})");
                var tabla = SpectreHelper.CrearTabla("SeatID", "SeatNumber", "Status");
                foreach (var s in grupo.OrderBy(x => x.SeatNumber))
                    SpectreHelper.AgregarFila(tabla, s.Id.ToString(), s.SeatNumber, s.Status);
                SpectreHelper.MostrarTabla(tabla);
            }
            SpectreHelper.MostrarInfo($"Total asientos: {asientos.Count}");
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task VerAsientosOcupadosAsync()
    {
        var vueloId = SpectreHelper.PedirEntero("ID del vuelo");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var ocupados = await scope.ServiceProvider
                .GetRequiredService<GetSeatDetailsByFlightUseCase>()
                .ExecuteAsync(vueloId, status: "Occupied");

            if (ocupados.Count == 0)
            {
                SpectreHelper.MostrarInfo("No hay asientos OCUPADOS en este vuelo.");
                SpectreHelper.EsperarTecla();
                return;
            }

            var tabla = SpectreHelper.CrearTabla("SeatID", "N° asiento", "Clase", "Estado");
            foreach (var s in ocupados)
                SpectreHelper.AgregarFila(tabla,
                    s.Id.ToString(),
                    s.SeatNumber,
                    s.FlightClassName,
                    s.Status);
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.MostrarInfo($"Total ocupados: {ocupados.Count}");
            SpectreHelper.EsperarTecla();
        });
    }

    private Task VerDisponibilidadPorClaseAsync()
    {
        var vueloId = SpectreHelper.PedirEntero("ID del vuelo");
        return ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var stats = await scope.ServiceProvider
                .GetRequiredService<GetSeatStatsByFlightUseCase>()
                .ExecuteAsync(vueloId);

            if (stats.Count == 0)
            {
                SpectreHelper.MostrarInfo("No hay asientos registrados para este vuelo.");
                SpectreHelper.EsperarTecla();
                return;
            }

            var tabla = SpectreHelper.CrearTabla(
                "ClaseID", "Código", "Nombre",
                "Total", "Disponibles", "Reservados", "Ocupados", "Bloqueados");

            foreach (var s in stats)
                SpectreHelper.AgregarFila(tabla,
                    s.FlightClassId.ToString(),
                    s.FlightClassCode,
                    s.FlightClassName,
                    s.Total.ToString(),
                    s.Available.ToString(),
                    s.Reserved.ToString(),
                    s.Occupied.ToString(),
                    s.Blocked.ToString());

            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private Task VerPorcentajeOcupacionAsync()
    {
        var vueloId = SpectreHelper.PedirEntero("ID del vuelo");
        return ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var stats = await scope.ServiceProvider
                .GetRequiredService<GetSeatStatsByFlightUseCase>()
                .ExecuteAsync(vueloId);

            if (stats.Count == 0)
            {
                SpectreHelper.MostrarInfo("No hay asientos registrados para este vuelo.");
                SpectreHelper.EsperarTecla();
                return;
            }

            var total = stats.Sum(s => s.Total);
            var ocupados = stats.Sum(s => s.Occupied);
            var porcentaje = total == 0 ? 0m : (decimal)ocupados * 100m / (decimal)total;

            var tabla = SpectreHelper.CrearTabla("Clase", "Ocupados", "Total", "% ocupación");
            foreach (var s in stats)
            {
                var pct = s.Total == 0 ? 0m : (decimal)s.Occupied * 100m / (decimal)s.Total;
                SpectreHelper.AgregarFila(tabla,
                    s.FlightClassName,
                    s.Occupied.ToString(),
                    s.Total.ToString(),
                    $"{pct:F2}%");
            }
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.MostrarInfo($"Ocupación total vuelo {vueloId}: {ocupados}/{total} ({porcentaje:F2}%)");
            SpectreHelper.EsperarTecla();
        });
    }

    private Task ConsultarReservasAsync()
    {
        SpectreHelper.MostrarInfo("Se abrirá el módulo de reservas (admin).");
        SpectreHelper.EsperarTecla();
        // Reutiliza el menú existente de reservas para consulta.
        return new AirTicketSystem.UI.Admin.Reservations.BookingMenu(_provider, _provider.GetRequiredService<SessionContext>())
            .MostrarAsync();
    }
}

