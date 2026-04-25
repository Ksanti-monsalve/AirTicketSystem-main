// src/modules/flightcrew/Domain/Repositories/IFlightCrewRepository.cs
using AirTicketSystem.modules.flightcrew.Domain.aggregate;

namespace AirTicketSystem.modules.flightcrew.Domain.Repositories;

public interface IFlightCrewRepository
{
    Task<FlightCrew?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<FlightCrew>> FindByVueloAsync(int vueloId);
    Task<IReadOnlyCollection<FlightCrew>> FindByTrabajadorAsync(int trabajadorId);
    Task<FlightCrew?> FindByVueloAndRolAsync(int vueloId, string rol);
    Task<bool> VueloTienePilotoAsync(int vueloId);
    Task<bool> VueloTieneCopiloAsync(int vueloId);
    Task<bool> ExistsByVueloAndTrabajadorAsync(int vueloId, int trabajadorId);
    Task<bool> ExistsByVueloAndRolAsync(int vueloId, string rol);
    Task SaveAsync(FlightCrew flightCrew);
    Task DeleteAsync(int id);
}