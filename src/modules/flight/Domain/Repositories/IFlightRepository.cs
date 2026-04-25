// src/modules/flight/Domain/Repositories/IFlightRepository.cs
using AirTicketSystem.modules.flight.Domain.aggregate;

namespace AirTicketSystem.modules.flight.Domain.Repositories;

public interface IFlightRepository
{
    Task<Flight?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<Flight>> FindAllAsync();
    Task<Flight?> FindByNumeroVueloAndFechaAsync(string numeroVuelo, DateTime fecha);
    Task<IReadOnlyCollection<Flight>> FindByRutaAsync(int rutaId);
    Task<IReadOnlyCollection<Flight>> FindByFechaAsync(DateTime fecha);
    Task<IReadOnlyCollection<Flight>> FindByOrigenAndDestinoAsync(
        int origenId, int destinoId, DateTime fecha);
    Task<IReadOnlyCollection<Flight>> FindProgramadosAsync();
    Task<IReadOnlyCollection<Flight>> FindConCheckinAbiertoAsync();
    Task<bool> ExistsByNumeroVueloAndFechaAsync(string numeroVuelo, DateTime fecha);
    Task SaveAsync(Flight flight);
    Task UpdateAsync(Flight flight);
    Task DeleteAsync(int id);
}