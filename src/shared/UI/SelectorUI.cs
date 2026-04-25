// src/shared/UI/SelectorUI.cs
// Selectores reutilizables para todos los menus:
// en lugar de "PedirEntero(ID de X)" muestra la lista y el usuario elige.
using Microsoft.Extensions.DependencyInjection;

using AirTicketSystem.modules.gender.Domain.aggregate;
using AirTicketSystem.modules.gender.Application.UseCases;
using AirTicketSystem.modules.documenttype.Domain.aggregate;
using AirTicketSystem.modules.documenttype.Application.UseCases;
using AirTicketSystem.modules.phonetype.Domain.aggregate;
using AirTicketSystem.modules.phonetype.Application.UseCases;
using AirTicketSystem.modules.emailtype.Domain.aggregate;
using AirTicketSystem.modules.emailtype.Application.UseCases;
using AirTicketSystem.modules.addresstype.Domain.aggregate;
using AirTicketSystem.modules.addresstype.Application.UseCases;
using AirTicketSystem.modules.contactrelationship.Domain.aggregate;
using AirTicketSystem.modules.contactrelationship.Application.UseCases;
using AirTicketSystem.modules.luggagetype.Domain.aggregate;
using AirTicketSystem.modules.luggagetype.Application.UseCases;
using AirTicketSystem.modules.serviceclass.Domain.aggregate;
using AirTicketSystem.modules.serviceclass.Application.UseCases;
using AirTicketSystem.modules.workertype.Domain.aggregate;
using AirTicketSystem.modules.workertype.Application.UseCases;
using AirTicketSystem.modules.specialty.Domain.aggregate;
using AirTicketSystem.modules.specialty.Application.UseCases;
using AirTicketSystem.modules.paymentmethod.Domain.aggregate;
using AirTicketSystem.modules.paymentmethod.Application.UseCases;
using AirTicketSystem.modules.luggagerestriction.Domain.aggregate;
using AirTicketSystem.modules.luggagerestriction.Application.UseCases;
using AirTicketSystem.modules.continent.Domain.aggregate;
using AirTicketSystem.modules.continent.Application.UseCases;
using AirTicketSystem.modules.country.Domain.aggregate;
using AirTicketSystem.modules.country.Application.UseCases;
using AirTicketSystem.modules.region.Domain.aggregate;
using AirTicketSystem.modules.region.Application.UseCases;
using AirTicketSystem.modules.department.Domain.aggregate;
using AirTicketSystem.modules.department.Application.UseCases;
using AirTicketSystem.modules.city.Domain.aggregate;
using AirTicketSystem.modules.city.Application.UseCases;
using AirTicketSystem.modules.airline.Domain.aggregate;
using AirTicketSystem.modules.airline.Application.UseCases;
using AirTicketSystem.modules.airport.Domain.aggregate;
using AirTicketSystem.modules.airport.Application.UseCases;
using AirTicketSystem.modules.terminal.Domain.aggregate;
using AirTicketSystem.modules.terminal.Application.UseCases;
using AirTicketSystem.modules.gate.Domain.aggregate;
using AirTicketSystem.modules.gate.Application.UseCases;
using AirTicketSystem.modules.aircraftmanufacturer.Domain.aggregate;
using AirTicketSystem.modules.aircraftmanufacturer.Application.UseCases;
using AirTicketSystem.modules.aircraftmodel.Domain.aggregate;
using AirTicketSystem.modules.aircraftmodel.Application.UseCases;
using AirTicketSystem.modules.aircraft.Domain.aggregate;
using AirTicketSystem.modules.aircraft.Application.UseCases;
using AirTicketSystem.modules.route.Domain.aggregate;
using AirTicketSystem.modules.route.Application.UseCases;
using AirTicketSystem.modules.fare.Domain.aggregate;
using AirTicketSystem.modules.fare.Application.UseCases;
using AirTicketSystem.modules.worker.Domain.aggregate;
using AirTicketSystem.modules.worker.Application.UseCases;
using AirTicketSystem.modules.pilotlicense.Domain.aggregate;
using AirTicketSystem.modules.pilotlicense.Application.UseCases;
using AirTicketSystem.modules.role.Domain.aggregate;
using AirTicketSystem.modules.role.Application.UseCases;
using AirTicketSystem.modules.person.Domain.aggregate;
using AirTicketSystem.modules.person.Application.UseCases;
using AirTicketSystem.modules.flight.Domain.aggregate;
using AirTicketSystem.modules.flight.Application.UseCases;
using AirTicketSystem.modules.booking.Domain.aggregate;
using AirTicketSystem.modules.booking.Application.UseCases;
using AirTicketSystem.modules.client.Domain.aggregate;
using AirTicketSystem.modules.client.Application.UseCases;

namespace AirTicketSystem.shared.UI;

public static class SelectorUI
{
    // ── Método genérico base ─────────────────────────────────────────────────

    /// <summary>
    /// Muestra una lista de opciones y retorna la seleccionada.
    /// Retorna null si la lista está vacía (muestra aviso al usuario).
    /// </summary>
    public static async Task<T?> SeleccionarAsync<T>(
        IServiceProvider provider,
        Func<IServiceProvider, Task<IReadOnlyCollection<T>>> consulta,
        Func<T, string> etiqueta,
        string titulo,
        string mensajeVacio = "No hay registros disponibles.") where T : class
    {
        await using var scope = provider.CreateAsyncScope();
        var lista = await consulta(scope.ServiceProvider);
        if (lista.Count == 0)
        {
            SpectreHelper.MostrarAdvertencia(mensajeVacio);
            SpectreHelper.EsperarTecla();
            return null;
        }
        return SpectreHelper.SeleccionarOpcion(titulo, lista, etiqueta);
    }

    // ── CATÁLOGOS ────────────────────────────────────────────────────────────

    public static async Task<Gender?> SeleccionarGeneroAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllGendersUseCase>().ExecuteAsync(),
            g => $"  {g.Nombre.Valor}",
            "Seleccione el género",
            "No hay géneros registrados. Créelos en Catálogos → Géneros.");

    public static async Task<DocumentType?> SeleccionarTipoDocumentoAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllDocumentTypesUseCase>().ExecuteAsync(),
            d => $"  {d.Descripcion.Valor}",
            "Seleccione el tipo de documento",
            "No hay tipos de documento. Créelos en Catálogos.");

    public static async Task<PhoneType?> SeleccionarTipoTelefonoAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllPhoneTypesUseCase>().ExecuteAsync(),
            t => $"  {t.Descripcion.Valor}",
            "Seleccione el tipo de teléfono",
            "No hay tipos de teléfono. Créelos en Catálogos.");

    public static async Task<EmailType?> SeleccionarTipoEmailAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllEmailTypesUseCase>().ExecuteAsync(),
            e => $"  {e.Descripcion.Valor}",
            "Seleccione el tipo de email",
            "No hay tipos de email. Créelos en Catálogos.");

    public static async Task<AddressType?> SeleccionarTipoDireccionAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllAddressTypesUseCase>().ExecuteAsync(),
            a => $"  {a.Descripcion.Valor}",
            "Seleccione el tipo de dirección",
            "No hay tipos de dirección. Créelos en Catálogos.");

    public static async Task<ContactRelationship?> SeleccionarRelacionContactoAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllContactRelationshipsUseCase>().ExecuteAsync(),
            r => $"  {r.Descripcion.Valor}",
            "Seleccione la relación de contacto",
            "No hay relaciones de contacto. Créelas en Catálogos.");

    public static async Task<LuggageType?> SeleccionarTipoEquipajeAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllLuggageTypesUseCase>().ExecuteAsync(),
            l => $"  {l.Nombre.Valor}",
            "Seleccione el tipo de equipaje",
            "No hay tipos de equipaje. Créelos en Catálogos.");

    public static async Task<ServiceClass?> SeleccionarClaseServicioAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllServiceClassesUseCase>().ExecuteAsync(),
            c => $"  [{c.Codigo.Valor}] {c.Nombre.Valor}",
            "Seleccione la clase de servicio",
            "No hay clases de servicio. Créelas en Catálogos.");

    public static async Task<WorkerType?> SeleccionarTipoTrabajadorAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllWorkerTypesUseCase>().ExecuteAsync(),
            w => $"  {w.Nombre.Valor}",
            "Seleccione el tipo de trabajador",
            "No hay tipos de trabajador. Créelos en Catálogos.");

    public static async Task<Specialty?> SeleccionarEspecialidadAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllSpecialtiesUseCase>().ExecuteAsync(),
            s => $"  {s.Nombre.Valor}",
            "Seleccione la especialidad",
            "No hay especialidades. Créelas en Catálogos.");

    public static async Task<PaymentMethod?> SeleccionarMetodoPagoAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllPaymentMethodsUseCase>().ExecuteAsync(),
            m => $"  {m.Nombre.Valor}",
            "Seleccione el método de pago",
            "No hay métodos de pago. Créelos en Catálogos → Métodos de pago.");

    // ── GEOGRAFÍA ────────────────────────────────────────────────────────────

    public static async Task<Continent?> SeleccionarContinenteAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllContinentsUseCase>().ExecuteAsync(),
            c => $"  {c.Nombre.Valor}",
            "Seleccione el continente",
            "No hay continentes registrados.");

    public static async Task<Country?> SeleccionarPaisAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllCountriesUseCase>().ExecuteAsync(),
            c => $"  [{c.CodigoIso2.Valor}] {c.Nombre.Valor}",
            "Seleccione el país",
            "No hay países registrados.");

    public static async Task<Region?> SeleccionarRegionAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllRegionsUseCase>().ExecuteAsync(),
            r => $"  {r.Nombre.Valor}",
            "Seleccione la región/departamento",
            "No hay regiones registradas.");

    public static async Task<Department?> SeleccionarDepartamentoAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllDepartmentsUseCase>().ExecuteAsync(),
            d => $"  {d.Nombre.Valor}",
            "Seleccione el departamento",
            "No hay departamentos registrados.");

    public static async Task<City?> SeleccionarCiudadAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllCitiesUseCase>().ExecuteAsync(),
            c => $"  {c.Nombre.Valor}",
            "Seleccione la ciudad",
            "No hay ciudades registradas.");

    // ── AEROLÍNEAS Y FLOTA ───────────────────────────────────────────────────

    public static async Task<AircraftManufacturer?> SeleccionarFabricanteAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllAircraftManufacturersUseCase>().ExecuteAsync(),
            m => $"  {m.Nombre.Valor}",
            "Seleccione el fabricante",
            "No hay fabricantes registrados.");

    public static async Task<AircraftModel?> SeleccionarModeloAvionAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllAircraftModelsUseCase>().ExecuteAsync(),
            m => $"  [{m.CodigoModelo.Valor}] {m.Nombre.Valor}",
            "Seleccione el modelo de avión",
            "No hay modelos registrados.");

    public static async Task<Airline?> SeleccionarAerolineaAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllAirlinesUseCase>().ExecuteAsync(),
            a => $"  [{a.CodigoIata.Valor}] {a.Nombre.Valor}",
            "Seleccione la aerolínea",
            "No hay aerolíneas registradas.");

    public static async Task<Airport?> SeleccionarAeropuertoAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllAirportsUseCase>().ExecuteAsync(),
            a => $"  [{a.CodigoIata.Valor}] {a.Nombre.Valor}",
            "Seleccione el aeropuerto",
            "No hay aeropuertos registrados.");

    public static async Task<Terminal?> SeleccionarTerminalAsync(IServiceProvider p, int aeropuertoId)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetTerminalsByAirportUseCase>().ExecuteAsync(aeropuertoId),
            t => $"  {t.Nombre.Valor}",
            "Seleccione la terminal",
            "No hay terminales en este aeropuerto.");

    public static async Task<Gate?> SeleccionarPuertaAsync(IServiceProvider p, int terminalId)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetGatesByTerminalUseCase>().ExecuteAsync(terminalId),
            g => $"  {g.Codigo.Valor}",
            "Seleccione la puerta de embarque",
            "No hay puertas en esta terminal.");

    public static async Task<Aircraft?> SeleccionarAvionAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllAircraftUseCase>().ExecuteAsync(),
            a => $"  [{a.Matricula.Valor}]",
            "Seleccione el avión",
            "No hay aviones registrados.");

    public static async Task<Route?> SeleccionarRutaAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllRoutesUseCase>().ExecuteAsync(),
            r => $"  Ruta #{r.Id}  Origen:{r.OrigenId} → Destino:{r.DestinoId}  [{r.AerolineaId}]",
            "Seleccione la ruta",
            "No hay rutas registradas.");

    public static async Task<Fare?> SeleccionarTarifaAsync(IServiceProvider p, int rutaId)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetActiveFaresByRouteUseCase>().ExecuteAsync(rutaId),
            f => $"  {f.Nombre.Valor}  —  {f.PrecioTotal.Valor:C}  [{f.ClaseServicioId}]",
            "Seleccione la tarifa",
            "No hay tarifas activas para esta ruta.");

    // ── PERSONAL ─────────────────────────────────────────────────────────────

    public static async Task<Worker?> SeleccionarTrabajadorAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetActiveWorkersUseCase>().ExecuteAsync(),
            w => $"  Trabajador #{w.Id}  PersonaID:{w.PersonaId}  TipoID:{w.TipoTrabajadorId}",
            "Seleccione el trabajador",
            "No hay trabajadores activos.");

    public static async Task<PilotLicense?> SeleccionarLicenciaAsync(IServiceProvider p, int trabajadorId)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetLicensesByWorkerUseCase>().ExecuteAsync(trabajadorId),
            l => $"  [{l.TipoLicencia.Valor}] {l.NumeroLicencia.Valor}  Vence:{l.FechaVencimiento.Valor:yyyy-MM-dd}",
            "Seleccione la licencia",
            "Este trabajador no tiene licencias.");

    // ── ROLES Y USUARIOS ─────────────────────────────────────────────────────

    public static async Task<Role?> SeleccionarRolAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllRolesUseCase>().ExecuteAsync(),
            r => $"  {r.Nombre.Valor}",
            "Seleccione el rol",
            "No hay roles registrados.");

    // ── PERSONAS ─────────────────────────────────────────────────────────────

    public static async Task<Person?> SeleccionarPersonaAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllPersonsUseCase>().ExecuteAsync(),
            per => $"  {per.Nombres.Valor} {per.Apellidos.Valor}  [{per.NumeroDoc.Valor}]",
            "Seleccione la persona",
            "No hay personas registradas.");

    // ── VUELOS ───────────────────────────────────────────────────────────────

    public static async Task<Flight?> SeleccionarVueloAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllFlightsUseCase>().ExecuteAsync(),
            v => $"  {v.NumeroVuelo.Valor}  Ruta:{v.RutaId}  Salida:{v.FechaSalida.Valor:yyyy-MM-dd HH:mm}  [{v.Estado.Valor}]",
            "Seleccione el vuelo",
            "No hay vuelos registrados.");

    public static async Task<Flight?> SeleccionarVueloProgramadoAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetScheduledFlightsUseCase>().ExecuteAsync(),
            v => $"  {v.NumeroVuelo.Valor}  Ruta:{v.RutaId}  Salida:{v.FechaSalida.Valor:yyyy-MM-dd HH:mm}",
            "Seleccione el vuelo programado",
            "No hay vuelos programados disponibles.");

    // ── CLIENTES ─────────────────────────────────────────────────────────────

    public static async Task<Client?> SeleccionarClienteAsync(IServiceProvider p)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetAllClientsUseCase>().ExecuteAsync(),
            c => $"  Cliente #{c.Id}  PersonaID:{c.PersonaId}  UsuarioID:{c.UsuarioId}",
            "Seleccione el cliente",
            "No hay clientes registrados.");

    // ── RESERVAS ─────────────────────────────────────────────────────────────

    public static async Task<Booking?> SeleccionarReservaAsync(IServiceProvider p, int clienteId)
        => await SeleccionarAsync(p,
            sp => sp.GetRequiredService<GetBookingsByClienteUseCase>().ExecuteAsync(clienteId),
            b => $"  [{b.CodigoReserva.Valor}]  {b.Estado.Valor}  {b.ValorTotal.Valor:C}",
            "Seleccione la reserva",
            "Este cliente no tiene reservas.");
}
