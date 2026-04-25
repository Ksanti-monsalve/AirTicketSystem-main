// src/modules/specialty/Application/Interfaces/ISpecialtyService.cs
using AirTicketSystem.modules.specialty.Domain.aggregate;

namespace AirTicketSystem.modules.specialty.Application.Interfaces;

public interface ISpecialtyService
{
    Task<Specialty> CreateAsync(string nombre, int? tipoTrabajadorId);
    Task<Specialty> GetByIdAsync(int id);
    Task<IReadOnlyCollection<Specialty>> GetAllAsync();
    Task<IReadOnlyCollection<Specialty>> GetByWorkerTypeAsync(int tipoTrabajadorId);
    Task<IReadOnlyCollection<Specialty>> GetGeneralesAsync();
    Task<Specialty> UpdateAsync(
        int id, string nombre, int? tipoTrabajadorId);
    Task DeleteAsync(int id);
}