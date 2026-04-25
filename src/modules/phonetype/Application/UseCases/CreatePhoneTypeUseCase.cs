// src/modules/phonetype/Application/UseCases/CreatePhoneTypeUseCase.cs
using AirTicketSystem.modules.phonetype.Domain.aggregate;
using AirTicketSystem.modules.phonetype.Domain.Repositories;
using AirTicketSystem.modules.phonetype.Domain.ValueObjects;

namespace AirTicketSystem.modules.phonetype.Application.UseCases;

public sealed class CreatePhoneTypeUseCase
{
    private readonly IPhoneTypeRepository _repository;

    public CreatePhoneTypeUseCase(IPhoneTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<PhoneType> ExecuteAsync(
        string descripcion,
        CancellationToken cancellationToken = default)
    {
        var descripcionVO = DescripcionPhoneType.Crear(descripcion);

        if (await _repository.ExistsByDescripcionAsync(descripcionVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un tipo de teléfono con la descripción '{descripcionVO.Valor}'.");

        var phoneType = PhoneType.Crear(descripcionVO.Valor);
        await _repository.SaveAsync(phoneType);
        return phoneType;
    }
}
