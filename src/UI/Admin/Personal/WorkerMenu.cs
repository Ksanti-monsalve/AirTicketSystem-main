// src/UI/Admin/Personal/WorkerMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.worker.Application.UseCases;
using AirTicketSystem.shared.UI;
using AirTicketSystem.modules.person.Domain.aggregate;
using AirTicketSystem.modules.workertype.Domain.aggregate;
using AirTicketSystem.modules.airport.Domain.aggregate;
using AirTicketSystem.modules.airline.Domain.aggregate;

namespace AirTicketSystem.UI.Admin.Personal;

public sealed class WorkerMenu
{
    private readonly IServiceProvider _provider;

    public WorkerMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Trabajadores");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Listar todos",
                    "Listar activos",
                    "Buscar por ID",
                    "Buscar por aerolínea",
                    "Buscar por aeropuerto",
                    "Crear trabajador",
                    "Actualizar salario",
                    "Actualizar aeropuerto base",
                    "Asignar especialidad",
                    "Remover especialidad",
                    "Activar trabajador",
                    "Desactivar trabajador",
                    "Eliminar trabajador",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Listar todos":             await ListarTodosAsync();            break;
                case "Listar activos":           await ListarActivosAsync();          break;
                case "Buscar por ID":            await BuscarPorIdAsync();            break;
                case "Buscar por aerolínea":     await ListarPorAerolineaAsync();     break;
                case "Buscar por aeropuerto":    await ListarPorAeropuertoAsync();    break;
                case "Crear trabajador":         await CrearAsync();                  break;
                case "Actualizar salario":       await ActualizarSalarioAsync();      break;
                case "Actualizar aeropuerto base": await ActualizarAeropuertoAsync(); break;
                case "Asignar especialidad":     await AsignarEspecialidadAsync();    break;
                case "Remover especialidad":     await RemoverEspecialidadAsync();    break;
                case "Activar trabajador":       await ActivarAsync();                break;
                case "Desactivar trabajador":    await DesactivarAsync();             break;
                case "Eliminar trabajador":      await EliminarAsync();               break;
                case "Volver":                   return;
            }
        }
    }

    private async Task ListarTodosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetAllWorkersUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin trabajadores registrados."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "PersonaID", "TipoTrabajID", "AeropuertoID", "AerolineaID", "Salario", "Activo");
            foreach (var w in lista)
                SpectreHelper.AgregarFila(tabla,
                    w.Id.ToString(), w.PersonaId.ToString(), w.TipoTrabajadorId.ToString(),
                    w.AeropuertoBaseId.ToString(), w.AerolineaId?.ToString() ?? "-",
                    w.Salario.Valor.ToString("C"), w.Activo.Valor ? "Sí" : "No");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarActivosAsync()
    {
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetActiveWorkersUseCase>().ExecuteAsync();
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin trabajadores activos."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "PersonaID", "TipoTrabajID", "AeropuertoID", "AerolineaID", "Salario");
            foreach (var w in lista)
                SpectreHelper.AgregarFila(tabla,
                    w.Id.ToString(), w.PersonaId.ToString(), w.TipoTrabajadorId.ToString(),
                    w.AeropuertoBaseId.ToString(), w.AerolineaId?.ToString() ?? "-",
                    w.Salario.Valor.ToString("C"));
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task BuscarPorIdAsync()
    {
        var id = SpectreHelper.PedirEntero("ID del trabajador");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var w = await scope.ServiceProvider.GetRequiredService<GetWorkerByIdUseCase>().ExecuteAsync(id);

            var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
            SpectreHelper.AgregarFila(tabla, "ID",               w.Id.ToString());
            SpectreHelper.AgregarFila(tabla, "PersonaID",        w.PersonaId.ToString());
            SpectreHelper.AgregarFila(tabla, "TipoTrabajadorID", w.TipoTrabajadorId.ToString());
            SpectreHelper.AgregarFila(tabla, "AeropuertoBase",   w.AeropuertoBaseId.ToString());
            SpectreHelper.AgregarFila(tabla, "AerolineaID",      w.AerolineaId?.ToString() ?? "-");
            SpectreHelper.AgregarFila(tabla, "UsuarioID",        w.UsuarioId?.ToString() ?? "-");
            SpectreHelper.AgregarFila(tabla, "Fecha contratación", w.FechaContratacion.Valor.ToString("yyyy-MM-dd"));
            SpectreHelper.AgregarFila(tabla, "Salario",          w.Salario.Valor.ToString("C"));
            SpectreHelper.AgregarFila(tabla, "Años en empresa",  w.AnosEnLaEmpresa.ToString());
            SpectreHelper.AgregarFila(tabla, "Activo",           w.Activo.Valor ? "Sí" : "No");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarPorAerolineaAsync()
    {
        var aerolinea = await SelectorUI.SeleccionarAerolineaAsync(_provider);
        if (aerolinea is null) {
            SpectreHelper.EsperarTecla();
            return;
        }
        var aerolineaId = aerolinea.Id;
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetWorkersByAirlineUseCase>().ExecuteAsync(aerolineaId);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin trabajadores para esta aerolínea."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "PersonaID", "TipoTrabajID", "Salario", "Activo");
            foreach (var w in lista)
                SpectreHelper.AgregarFila(tabla, w.Id.ToString(), w.PersonaId.ToString(),
                    w.TipoTrabajadorId.ToString(), w.Salario.Valor.ToString("C"), w.Activo.Valor ? "Sí" : "No");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task ListarPorAeropuertoAsync()
    {
        var aeropuerto = await SelectorUI.SeleccionarAeropuertoAsync(_provider);
        if (aeropuerto is null) {
            SpectreHelper.EsperarTecla();
            return;
        }
        var aeropuertoId = aeropuerto.Id;
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var lista = await scope.ServiceProvider.GetRequiredService<GetWorkersByAirportUseCase>().ExecuteAsync(aeropuertoId);
            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin trabajadores en este aeropuerto."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "PersonaID", "TipoTrabajID", "AerolineaID", "Salario");
            foreach (var w in lista)
                SpectreHelper.AgregarFila(tabla, w.Id.ToString(), w.PersonaId.ToString(),
                    w.TipoTrabajadorId.ToString(), w.AerolineaId?.ToString() ?? "-", w.Salario.Valor.ToString("C"));
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task CrearAsync()
    {
        SpectreHelper.MostrarSubtitulo("Crear Trabajador");
        var persona = await SelectorUI.SeleccionarPersonaAsync(_provider);
        if (persona is null) {
            SpectreHelper.EsperarTecla();
            return;
        }
        var personaId = persona.Id;

        var tipoTrabajador = await SelectorUI.SeleccionarTipoTrabajadorAsync(_provider);
        if (tipoTrabajador is null) {
            SpectreHelper.EsperarTecla();
            return;
        }
        var tipoTrabajadorId = tipoTrabajador.Id;

        var aeropuerto = await SelectorUI.SeleccionarAeropuertoAsync(_provider);
        if (aeropuerto is null) {
            SpectreHelper.EsperarTecla();
            return;
        }
        var aeropuertoId = aeropuerto.Id;

        var fechaStr         = SpectreHelper.PedirTexto("Fecha de contratación (yyyy-MM-dd)");
        var salarioStr       = SpectreHelper.PedirTexto("Salario mensual");
        var aerolineaStr     = SpectreHelper.PedirTexto("ID aerolínea (opcional, Enter para omitir)");

        if (!DateOnly.TryParse(fechaStr, out var fecha))
        {
            SpectreHelper.MostrarError("Fecha inválida.");
            SpectreHelper.EsperarTecla(); return;
        }
        if (!decimal.TryParse(salarioStr, out var salario))
        {
            SpectreHelper.MostrarError("Salario inválido.");
            SpectreHelper.EsperarTecla(); return;
        }
        int? aerolineaId = int.TryParse(aerolineaStr, out var a) && a > 0 ? a : null;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var w = await scope.ServiceProvider.GetRequiredService<CreateWorkerUseCase>()
                .ExecuteAsync(personaId, tipoTrabajadorId, aeropuertoId, fecha, salario, aerolineaId);
            SpectreHelper.MostrarExito($"Trabajador creado (ID {w.Id}).");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ActualizarSalarioAsync()
    {
        var worker = await SelectorUI.SeleccionarTrabajadorAsync(_provider);
        if (worker is null) {
            SpectreHelper.EsperarTecla();
            return;
        }
        var id = worker.Id;
        var salarioStr = SpectreHelper.PedirTexto("Nuevo salario");
        if (!decimal.TryParse(salarioStr, out var salario)) { SpectreHelper.MostrarError("Valor inválido."); SpectreHelper.EsperarTecla(); return; }

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<UpdateWorkerSalaryUseCase>().ExecuteAsync(id, salario);
            SpectreHelper.MostrarExito("Salario actualizado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ActualizarAeropuertoAsync()
    {
        var worker = await SelectorUI.SeleccionarTrabajadorAsync(_provider);
        if (worker is null) {
            SpectreHelper.EsperarTecla();
            return;
        }
        var id = worker.Id;

        var aeropuerto = await SelectorUI.SeleccionarAeropuertoAsync(_provider);
        if (aeropuerto is null) {
            SpectreHelper.EsperarTecla();
            return;
        }
        var aeropuertoId = aeropuerto.Id;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<UpdateWorkerAirportUseCase>().ExecuteAsync(id, aeropuertoId);
            SpectreHelper.MostrarExito("Aeropuerto base actualizado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task AsignarEspecialidadAsync()
    {
        var worker = await SelectorUI.SeleccionarTrabajadorAsync(_provider);
        if (worker is null) {
            SpectreHelper.EsperarTecla();
            return;
        }
        var workerId = worker.Id;

        var especialidad = await SelectorUI.SeleccionarEspecialidadAsync(_provider);
        if (especialidad is null) {
            SpectreHelper.EsperarTecla();
            return;
        }
        var especialidadId = especialidad.Id;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<AssignWorkerSpecialtyUseCase>().ExecuteAsync(workerId, especialidadId);
            SpectreHelper.MostrarExito("Especialidad asignada.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task RemoverEspecialidadAsync()
    {
        var workerSpecialtyId = SpectreHelper.PedirEntero("ID de la asignación de especialidad (WorkerSpecialtyID)");

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<RemoveWorkerSpecialtyUseCase>().ExecuteAsync(workerSpecialtyId);
            SpectreHelper.MostrarExito("Especialidad removida.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task ActivarAsync()
    {
        var worker = await SelectorUI.SeleccionarTrabajadorAsync(_provider);
        if (worker is null) {
            SpectreHelper.EsperarTecla();
            return;
        }
        var id = worker.Id;
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<ActivateWorkerUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Trabajador activado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task DesactivarAsync()
    {
        var worker = await SelectorUI.SeleccionarTrabajadorAsync(_provider);
        if (worker is null) {
            SpectreHelper.EsperarTecla();
            return;
        }
        var id = worker.Id;
        if (!SpectreHelper.Confirmar("¿Confirma desactivar?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeactivateWorkerUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Trabajador desactivado.");
        });
        SpectreHelper.EsperarTecla();
    }

    private async Task EliminarAsync()
    {
        var worker = await SelectorUI.SeleccionarTrabajadorAsync(_provider);
        if (worker is null) {
            SpectreHelper.EsperarTecla();
            return;
        }
        var id = worker.Id;
        if (!SpectreHelper.Confirmar("¿Confirma la eliminación?")) { SpectreHelper.EsperarTecla(); return; }
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<DeleteWorkerUseCase>().ExecuteAsync(id);
            SpectreHelper.MostrarExito("Trabajador eliminado.");
        });
        SpectreHelper.EsperarTecla();
    }
}
