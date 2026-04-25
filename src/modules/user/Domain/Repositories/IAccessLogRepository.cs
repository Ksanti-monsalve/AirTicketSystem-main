// src/modules/user/Domain/Repositories/IAccessLogRepository.cs
using AirTicketSystem.modules.user.Domain.aggregate;

namespace AirTicketSystem.modules.user.Domain.Repositories;

public interface IAccessLogRepository
{
    Task<AccessLog?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<AccessLog>> FindByUsuarioAsync(int usuarioId);
    Task<IReadOnlyCollection<AccessLog>> FindByUsuarioAndTipoAsync(int usuarioId, string tipo);
    Task<AccessLog?> FindUltimoLoginAsync(int usuarioId);
    Task<int> ContarIntentosFallidosRecientesAsync(int usuarioId, DateTime desde);
    Task SaveAsync(AccessLog log);
}
