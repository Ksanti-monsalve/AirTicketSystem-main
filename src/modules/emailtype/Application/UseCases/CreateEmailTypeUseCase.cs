// src/modules/emailtype/Application/UseCases/CreateEmailTypeUseCase.cs
using AirTicketSystem.modules.emailtype.Domain.aggregate;
using AirTicketSystem.modules.emailtype.Domain.Repositories;
using AirTicketSystem.modules.emailtype.Domain.ValueObjects;

namespace AirTicketSystem.modules.emailtype.Application.UseCases;

public sealed class CreateEmailTypeUseCase
{
    private readonly IEmailTypeRepository _repository;

    public CreateEmailTypeUseCase(IEmailTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<EmailType> ExecuteAsync(
        string descripcion,
        CancellationToken cancellationToken = default)
    {
        var descripcionVO = DescripcionEmailType.Crear(descripcion);

        if (await _repository.ExistsByDescripcionAsync(descripcionVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un tipo de email con la descripción '{descripcionVO.Valor}'.");

        var emailType = EmailType.Crear(descripcionVO.Valor);
        await _repository.SaveAsync(emailType);
        return emailType;
    }
}
