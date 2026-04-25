// src/modules/emailtype/Domain/Repositories/IEmailTypeRepository.cs
using AirTicketSystem.modules.emailtype.Domain.aggregate;

namespace AirTicketSystem.modules.emailtype.Domain.Repositories;

public interface IEmailTypeRepository
{
    Task<EmailType?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<EmailType>> FindAllAsync();
    Task<bool> ExistsByDescripcionAsync(string descripcion);
    Task SaveAsync(EmailType emailType);
    Task UpdateAsync(EmailType emailType);
    Task DeleteAsync(int id);
}
