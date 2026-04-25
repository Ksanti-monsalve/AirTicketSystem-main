// src/modules/gender/Application/Interfaces/IGenderService.cs
using AirTicketSystem.modules.gender.Domain.aggregate;

namespace AirTicketSystem.modules.gender.Application.Interfaces;

public interface IGenderService
{
    Task<Gender> CreateAsync(string nombre);
    Task<Gender> GetByIdAsync(int id);
    Task<IReadOnlyCollection<Gender>> GetAllAsync();
    Task<Gender> UpdateAsync(int id, string nombre);
    Task DeleteAsync(int id);
}
