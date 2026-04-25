// src/modules/serviceclass/Application/Interfaces/IServiceClassService.cs
using AirTicketSystem.modules.serviceclass.Domain.aggregate;

namespace AirTicketSystem.modules.serviceclass.Application.Interfaces;

public interface IServiceClassService
{
    Task<ServiceClass> CreateAsync(
        string nombre, string codigo, string? descripcion);
    Task<ServiceClass> GetByIdAsync(int id);
    Task<IReadOnlyCollection<ServiceClass>> GetAllAsync();
    Task<ServiceClass> UpdateAsync(
        int id, string nombre, string? descripcion);
    Task DeleteAsync(int id);
}