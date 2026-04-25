// src/UI/Admin/ClientesUsuarios/AccessLogMenu.cs
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.UI;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.modules.user.Domain.Repositories;

namespace AirTicketSystem.UI.Admin.ClientesUsuarios;

public sealed class AccessLogMenu
{
    private readonly IServiceProvider _provider;

    public AccessLogMenu(IServiceProvider provider) => _provider = provider;

    public async Task MostrarAsync()
    {
        while (true)
        {
            SpectreHelper.MostrarTitulo("Log de Accesos");

            var opcion = SpectreHelper.SeleccionarOpcionTexto("Seleccione una acción",
                [
                    "Ver accesos de usuario",
                    "Ver último login de usuario",
                    "Ver intentos fallidos recientes",
                    "Volver"
                ]);

            switch (opcion)
            {
                case "Ver accesos de usuario":          await VerAccesosAsync();         break;
                case "Ver último login de usuario":     await VerUltimoLoginAsync();     break;
                case "Ver intentos fallidos recientes": await VerIntentosFallidosAsync(); break;
                case "Volver": return;
            }
        }
    }

    private async Task VerAccesosAsync()
    {
        var usuarioId = SpectreHelper.PedirEntero("ID del usuario");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var repo = scope.ServiceProvider.GetRequiredService<IAccessLogRepository>();
            var lista = await repo.FindByUsuarioAsync(usuarioId);

            if (lista.Count == 0) { SpectreHelper.MostrarInfo("Sin registros para este usuario."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("ID", "Tipo", "Fecha acceso", "IP");
            foreach (var log in lista.OrderByDescending(l => l.FechaAcceso.Valor))
                SpectreHelper.AgregarFila(tabla,
                    log.Id.ToString(), log.Tipo.Valor,
                    log.FechaAcceso.Valor.ToString("yyyy-MM-dd HH:mm:ss"),
                    log.IpAddress?.Valor ?? "-");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task VerUltimoLoginAsync()
    {
        var usuarioId = SpectreHelper.PedirEntero("ID del usuario");
        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var repo = scope.ServiceProvider.GetRequiredService<IAccessLogRepository>();
            var log = await repo.FindUltimoLoginAsync(usuarioId);

            if (log is null) { SpectreHelper.MostrarInfo("Sin registros de login para este usuario."); SpectreHelper.EsperarTecla(); return; }

            var tabla = SpectreHelper.CrearTabla("Campo", "Valor");
            SpectreHelper.AgregarFila(tabla, "Fecha",      log.FechaAcceso.Valor.ToString("yyyy-MM-dd HH:mm:ss"));
            SpectreHelper.AgregarFila(tabla, "Tipo",       log.Tipo.Valor);
            SpectreHelper.AgregarFila(tabla, "IP",         log.IpAddress?.Valor ?? "-");
            SpectreHelper.MostrarTabla(tabla);
            SpectreHelper.EsperarTecla();
        });
    }

    private async Task VerIntentosFallidosAsync()
    {
        var usuarioId  = SpectreHelper.PedirEntero("ID del usuario");
        var horasStr   = SpectreHelper.PedirTexto("Últimas N horas (ej: 24)");
        if (!int.TryParse(horasStr, out var horas) || horas <= 0) horas = 24;

        await ConsoleErrorHandler.ExecuteAsync(async () =>
        {
            await using var scope = _provider.CreateAsyncScope();
            var repo  = scope.ServiceProvider.GetRequiredService<IAccessLogRepository>();
            var desde = DateTime.UtcNow.AddHours(-horas);
            var count = await repo.ContarIntentosFallidosRecientesAsync(usuarioId, desde);

            SpectreHelper.MostrarInfo($"Intentos fallidos en las últimas {horas}h: {count}");
            SpectreHelper.EsperarTecla();
        });
    }
}
