// src/modules/emailtype/Application/UseCases/UpdateEmailTypeUseCase.cs
using AirTicketSystem.modules.emailtype.Domain.aggregate;
using AirTicketSystem.modules.emailtype.Domain.Repositories;
using AirTicketSystem.modules.emailtype.Domain.ValueObjects;

namespace AirTicketSystem.modules.emailtype.Application.UseCases;

public sealed class UpdateEmailTypeUseCase
{
    private readonly IEmailTypeRepository _repository;

    public UpdateEmailTypeUseCase(IEmailTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<EmailType> ExecuteAsync(
        int id,
        string descripcion,
        CancellationToken cancellationToken = default)
    {
        var emailType = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de email con ID {id}.");

        var descripcionVO = DescripcionEmailType.Crear(descripcion);

        if (!string.Equals(descripcionVO.Valor, emailType.Descripcion.Valor, StringComparison.OrdinalIgnoreCase) &&
            await _repository.ExistsByDescripcionAsync(descripcionVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otro tipo de email con la descripción '{descripcionVO.Valor}'.");

        emailType.ActualizarDescripcion(descripcionVO.Valor);
        await _repository.UpdateAsync(emailType);
        return emailType;
    }
}
