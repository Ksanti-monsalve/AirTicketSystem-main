// src/modules/flighthistory/Domain/Repositories/IFlightHistoryRepository.cs
using AirTicketSystem.modules.flighthistory.Domain.aggregate;

namespace AirTicketSystem.modules.flighthistory.Domain.Repositories;

public interface IFlightHistoryRepository
{
    Task<IReadOnlyCollection<FlightHistory>> FindByVueloAsync(int vueloId);
    Task<FlightHistory?> FindUltimoCambioByVueloAsync(int vueloId);
    Task SaveAsync(FlightHistory flightHistory);
}