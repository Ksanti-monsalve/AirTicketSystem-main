// src/modules/city/Application/Interfaces/ICityService.cs
using AirTicketSystem.modules.city.Domain.aggregate;

namespace AirTicketSystem.modules.city.Application.Interfaces;

public interface ICityService
{
    Task<City> CreateAsync(int departamentoId, string nombre, string? codigoPostal);
    Task<City> GetByIdAsync(int id);
    Task<IReadOnlyCollection<City>> GetAllAsync();
    Task<IReadOnlyCollection<City>> GetByDepartmentAsync(int departamentoId);
    Task<City> UpdateAsync(int id, string nombre, string? codigoPostal);
    Task DeleteAsync(int id);
}
