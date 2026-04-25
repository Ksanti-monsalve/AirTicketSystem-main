// src/modules/specialty/Domain/Repositories/ISpecialtyRepository.cs
using AirTicketSystem.modules.specialty.Domain.aggregate;

namespace AirTicketSystem.modules.specialty.Domain.Repositories;

public interface ISpecialtyRepository
{
    Task<Specialty?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<Specialty>> FindAllAsync();
    Task<IReadOnlyCollection<Specialty>> FindByTipoTrabajadorAsync(int tipoTrabajadorId);
    Task<IReadOnlyCollection<Specialty>> FindGeneralesAsync();
    Task<bool> ExistsByNombreAsync(string nombre);
    Task SaveAsync(Specialty specialty);
    Task UpdateAsync(Specialty specialty);
    Task DeleteAsync(int id);
}