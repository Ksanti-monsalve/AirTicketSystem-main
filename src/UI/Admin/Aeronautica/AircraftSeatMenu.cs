// src/UI/Admin/Aeronautica/AircraftSeatMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.aircraftseat.Application.UseCases;
using AirTicketSystem.modules.aircraft.Application.UseCases;
using AirTicketSystem.modules.serviceclass.Application.UseCases;

namespace AirTicketSystem.UI.Admin.Aeronautica;

public sealed class AircraftSeatMenu
{
    private readonly IServiceProvider _provider;

    public AircraftSeatMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Asientos de Aeronave");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Listar asientos de aeronave",
                    "Listar por aeronave y clase",
                    "Crear asiento",
                    "Editar condiciones",
                    "Activar asiento",
                    "Desactivar asiento",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Listar asientos de aeronave": await ListarPorAvionAsync();          break;
                case "Listar por aeronave y clase": await ListarPorAvionYClaseAsync();    break;
                case "Crear asiento":               await CrearAsync();                  break;
                case "Editar condiciones":          await EditarAsync();                 break;
                case "Activar asiento":             await ActivarAsync();                break;
                case "Desactivar asiento":          await DesactivarAsync();             break;
                case "Volver":                      return;
            }
        }
    }

    private async Task ListarPorAvionAsync()
    {
        await MostrarAvionesAsync();
        var avionId = SpectreHelper.PedirEntero("ID de la aeronave");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider
                .GetRequiredService<GetSeatsByAircraftUseCase>().ExecuteAsync(avionId);

            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin asientos registrados."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Código", "Fila", "Columna", "Clase", "Ventana", "Pasillo", "Activo", "Costo");
            foreach (var s in lista)
                SpectreHelper.AgregarFila(tabla,
                    s.Id.ToString(), s.CodigoAsiento.Valor,
                    s.Fila.Valor.ToString(), s.Columna.Valor.ToString(),
                    s.ClaseServicioId.ToString(),
                    s.EsVentana.Valor ? "Sí" : "No",
                    s.EsPasillo.Valor ? "Sí" : "No",
                    s.Activo.Valor ? "Sí" : "No",
                    s.CostoSeleccion.Valor.ToString("C2"));

            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarPorAvionYClaseAsync()
    {
        await MostrarAvionesAsync();
        var avionId  = SpectreHelper.PedirEntero("ID de la aeronave");
        await MostrarClasesAsync();
        var claseId  = SpectreHelper.PedirEntero("ID de la clase de servicio");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider
                .GetRequiredService<GetSeatsByAircraftAndClassUseCase>().ExecuteAsync(avionId, claseId);

            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin resultados."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Código", "Fila", "Columna", "Ubicación", "Activo");
            foreach (var s in lista)
                SpectreHelper.AgregarFila(tabla,
                    s.Id.ToString(), s.CodigoAsiento.Valor,
                    s.Fila.Valor.ToString(), s.Columna.Valor.ToString(),
                    s.UbicacionDescriptiva,
                    s.Activo.Valor ? "Sí" : "No");

            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Crear Asiento");

        await MostrarAvionesAsync();
        var avionId = SpectreHelper.PedirEntero("ID de la aeronave");

        await MostrarClasesAsync();
        var claseId = SpectreHelper.PedirEntero("ID de la clase de servicio");

        var fila        = SpectreHelper.PedirEntero("Número de fila");
        var columnaStr  = SpectreHelper.PedirTexto("Letra de columna (A-K)");
        var esVentana   = SpectreHelper.Confirmar("¿Es asiento de ventana?");
        var esPasillo   = !esVentana && SpectreHelper.Confirmar("¿Es asiento de pasillo?");
        var costo       = SpectreHelper.PedirDecimal("Costo de selección (0 si es gratis)");

        if (columnaStr.Length != 1)
        {
            SpectreHelper.MostrarError("La columna debe ser una sola letra."); SpectreHelper.EsperarTecla(); return;
        }

        var columna = char.ToUpperInvariant(columnaStr[0]);

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider
                .GetRequiredService<CreateAircraftSeatUseCase>()
                .ExecuteAsync(avionId, claseId, fila, columna, esVentana, esPasillo, costo);
            SpectreHelper.MostrarExito($"Asiento '{r.CodigoAsiento.Valor}' creado (ID {r.Id}).");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task EditarAsync()
    {
        SpectreHelper.MostrarSubtitulo("Editar Condiciones del Asiento");
        var id        = SpectreHelper.PedirEntero("ID del asiento");
        var esVentana = SpectreHelper.Confirmar("¿Es asiento de ventana?");
        var esPasillo = !esVentana && SpectreHelper.Confirmar("¿Es asiento de pasillo?");
        var costo     = SpectreHelper.PedirDecimal("Costo de selección (0 si es gratis)");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var r = await scope.ServiceProvider
                .GetRequiredService<UpdateAircraftSeatUseCase>()
                .ExecuteAsync(id, esVentana, esPasillo, costo);
            SpectreHelper.MostrarExito($"Asiento '{r.CodigoAsiento.Valor}' actualizado.");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task ActivarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del asiento a activar");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider
                .GetRequiredService<ActivateAircraftSeatUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Asiento activado.");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task DesactivarAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del asiento a desactivar");

        if (!SpectreHelper.Confirmar("¿Confirma desactivar este asiento?"))
        { SpectreHelper.MostrarInfo("Cancelado."); SpectreHelper.EsperarTecla(); return; }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider
                .GetRequiredService<DeactivateAircraftSeatUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Asiento desactivado.");
        });

        SpectreHelper.EsperarTecla();
    }

    private async Task MostrarAvionesAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider
                .GetRequiredService<GetAllAircraftUseCase>().ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "Matrícula", "Estado", "AerolineaID");
            foreach (var a in lista)
                SpectreHelper.AgregarFila(tabla, a.Id.ToString(), a.Matricula.Valor,
                    a.Estado.Valor, a.AerolineaId.ToString());
            SpectreHelper.MostrarTabla(tabla);
        });
    }

    private async Task MostrarClasesAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider
                .GetRequiredService<GetAllServiceClassesUseCase>().ExecuteAsync();
            if (lista.Count == 0) return;
            var tabla = SpectreHelper.CrearTabla("ID", "Clase", "Código");
            foreach (var c in lista)
                SpectreHelper.AgregarFila(tabla, c.Id.ToString(), c.Nombre.Valor, c.Codigo.Valor);
            SpectreHelper.MostrarTabla(tabla);
        });
    }
}
