// src/modules/gender/Domain/Repositories/IGenderRepository.cs
using AirTicketSystem.modules.gender.Domain.aggregate;

namespace AirTicketSystem.modules.gender.Domain.Repositories;

public interface IGenderRepository
{
    Task<Gender?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<Gender>> FindAllAsync();
    Task<bool> ExistsByNombreAsync(string nombre);
    Task SaveAsync(Gender gender);
    Task UpdateAsync(Gender gender);
    Task DeleteAsync(int id);
}
