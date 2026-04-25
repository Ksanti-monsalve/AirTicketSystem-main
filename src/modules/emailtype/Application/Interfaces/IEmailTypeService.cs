// src/modules/emailtype/Application/Interfaces/IEmailTypeService.cs
using AirTicketSystem.modules.emailtype.Domain.aggregate;

namespace AirTicketSystem.modules.emailtype.Application.Interfaces;

public interface IEmailTypeService
{
    Task<EmailType> CreateAsync(string descripcion);
    Task<EmailType> GetByIdAsync(int id);
    Task<IReadOnlyCollection<EmailType>> GetAllAsync();
    Task<EmailType> UpdateAsync(int id, string descripcion);
    Task DeleteAsync(int id);
}
