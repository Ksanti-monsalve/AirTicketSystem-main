// src/modules/luggage/Domain/Repositories/ILuggageRepository.cs
using AirTicketSystem.modules.luggage.Domain.aggregate;

namespace AirTicketSystem.modules.luggage.Domain.Repositories;

public interface ILuggageRepository
{
    Task<Luggage?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<Luggage>> FindByPasajeroReservaAsync(int pasajeroReservaId);
    Task<IReadOnlyCollection<Luggage>> FindByVueloAsync(int vueloId);
    Task<Luggage?> FindByCodigoEquipajeAsync(string codigoEquipaje);
    Task<IReadOnlyCollection<Luggage>> FindByEstadoAsync(string estado);
    Task<IReadOnlyCollection<Luggage>> FindConIncidenciasAsync();
    Task<bool> ExistsByCodigoEquipajeAsync(string codigoEquipaje);
    Task SaveAsync(Luggage luggage);
    Task UpdateAsync(Luggage luggage);
}
