// src/modules/seatavailability/Domain/Repositories/ISeatAvailabilityRepository.cs
using AirTicketSystem.modules.seatavailability.Domain.aggregate;

namespace AirTicketSystem.modules.seatavailability.Domain.Repositories;

public interface ISeatAvailabilityRepository
{
    Task<IReadOnlyCollection<SeatAvailabilityDetail>> FindDetallesByVueloAsync(
        int vueloId, string? estado = null);

    // Requisito EXAMEN (FASE 10): asientos por reserva
    Task<IReadOnlyCollection<SeatAvailabilityDetail>> FindDetallesByReservaAsync(int reservaId);
    Task<IReadOnlyCollection<ServiceClassAvailability>> FindClasesDisponiblesByVueloAsync(int vueloId);
    Task<IReadOnlyCollection<SeatAvailabilityDetail>> FindDetallesDisponiblesByVueloAndClaseAsync(
        int vueloId, int claseServicioId);
    Task<SeatAvailability?> FindByVueloAndAsientoAsync(int vueloId, int asientoId);
    Task<IReadOnlyCollection<SeatAvailability>> FindByVueloAsync(int vueloId);
    Task<IReadOnlyCollection<SeatAvailability>> FindDisponiblesByVueloAsync(int vueloId);
    Task<IReadOnlyCollection<SeatAvailability>> FindDisponiblesByVueloAndClaseAsync(
        int vueloId, int claseServicioId);
    Task<int> ContarDisponiblesByVueloAsync(int vueloId);
    Task<bool> AsientoDisponibleAsync(int vueloId, int asientoId);
    Task<bool> TryReserveDisponibilidadAsync(int disponibilidadId);
    Task<bool> TryOccupyDisponibilidadAsync(int disponibilidadId);
    Task<bool> TryReleaseDisponibilidadAsync(int disponibilidadId);
    Task SetReservaIdAsync(int disponibilidadId, int reservaId);
    Task SetTiqueteIdAsync(int disponibilidadId, int tiqueteId);
    Task<SeatAvailabilityDetail?> FindDetalleByDisponibilidadIdAsync(int disponibilidadId);
    Task<IReadOnlyCollection<ServiceClassStateStats>> FindStatsByVueloAsync(int vueloId);
    Task SaveAllAsync(IEnumerable<SeatAvailability> asientos);
    Task UpdateAsync(SeatAvailability seatAvailability);
}

public sealed record SeatAvailabilityDetail(
    int DisponibilidadId,
    int VueloId,
    int AsientoId,
    string NumeroAsiento,
    int ClaseServicioId,
    string ClaseServicioNombre,
    string Estado);

public sealed record ServiceClassAvailability(
    int ClaseServicioId,
    string ClaseServicioNombre,
    string ClaseServicioCodigo,
    int CantidadDisponible);

public sealed record ServiceClassStateStats(
    int ClaseServicioId,
    string ClaseServicioNombre,
    string ClaseServicioCodigo,
    int Total,
    int Disponibles,
    int Reservados,
    int Ocupados,
    int Bloqueados);