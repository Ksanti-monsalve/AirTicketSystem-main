// src/modules/emailtype/Application/UseCases/GetEmailTypeByIdUseCase.cs
using AirTicketSystem.modules.emailtype.Domain.aggregate;
using AirTicketSystem.modules.emailtype.Domain.Repositories;

namespace AirTicketSystem.modules.emailtype.Application.UseCases;

public sealed class GetEmailTypeByIdUseCase
{
    private readonly IEmailTypeRepository _repository;

    public GetEmailTypeByIdUseCase(IEmailTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<EmailType> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de email con ID {id}.");
    }
}
