// src/modules/emailtype/Application/UseCases/GetAllEmailTypesUseCase.cs
using AirTicketSystem.modules.emailtype.Domain.aggregate;
using AirTicketSystem.modules.emailtype.Domain.Repositories;

namespace AirTicketSystem.modules.emailtype.Application.UseCases;

public sealed class GetAllEmailTypesUseCase
{
    private readonly IEmailTypeRepository _repository;

    public GetAllEmailTypesUseCase(IEmailTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<EmailType>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return await _repository.FindAllAsync();
    }
}
