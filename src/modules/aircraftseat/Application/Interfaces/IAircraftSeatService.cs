// src/modules/aircraftseat/Application/Interfaces/IAircraftSeatService.cs
using AirTicketSystem.modules.aircraftseat.Domain.aggregate;

namespace AirTicketSystem.modules.aircraftseat.Application.Interfaces;

public interface IAircraftSeatService
{
    Task<AircraftSeat> CreateAsync(
        int avionId, int claseServicioId,
        int fila, char columna,
        bool esVentana, bool esPasillo,
        decimal costoSeleccion);
    Task<AircraftSeat> GetByIdAsync(int id);
    Task<IReadOnlyCollection<AircraftSeat>> GetByAircraftAsync(int avionId);
    Task<IReadOnlyCollection<AircraftSeat>> GetByAircraftAndClassAsync(
        int avionId, int claseServicioId);
    Task<AircraftSeat> UpdateAsync(
        int id, bool esVentana, bool esPasillo, decimal costoSeleccion);
    Task ActivateAsync(int id);
    Task DeactivateAsync(int id);
    Task DeleteAsync(int id);
}