// src/modules/emailtype/Application/Services/EmailTypeService.cs
using AirTicketSystem.modules.emailtype.Application.Interfaces;
using AirTicketSystem.modules.emailtype.Application.UseCases;
using AirTicketSystem.modules.emailtype.Domain.aggregate;

namespace AirTicketSystem.modules.emailtype.Application.Services;

public sealed class EmailTypeService : IEmailTypeService
{
    private readonly CreateEmailTypeUseCase _create;
    private readonly GetEmailTypeByIdUseCase _getById;
    private readonly GetAllEmailTypesUseCase _getAll;
    private readonly UpdateEmailTypeUseCase _update;
    private readonly DeleteEmailTypeUseCase _delete;

    public EmailTypeService(
        CreateEmailTypeUseCase create,
        GetEmailTypeByIdUseCase getById,
        GetAllEmailTypesUseCase getAll,
        UpdateEmailTypeUseCase update,
        DeleteEmailTypeUseCase delete)
    {
        _create  = create;
        _getById = getById;
        _getAll  = getAll;
        _update  = update;
        _delete  = delete;
    }

    public Task<EmailType> CreateAsync(string descripcion)
        => _create.ExecuteAsync(descripcion);

    public Task<EmailType> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<EmailType>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<EmailType> UpdateAsync(int id, string descripcion)
        => _update.ExecuteAsync(id, descripcion);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}
