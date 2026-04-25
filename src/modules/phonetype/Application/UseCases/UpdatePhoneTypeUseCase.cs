// src/modules/phonetype/Application/UseCases/UpdatePhoneTypeUseCase.cs
using AirTicketSystem.modules.phonetype.Domain.aggregate;
using AirTicketSystem.modules.phonetype.Domain.Repositories;
using AirTicketSystem.modules.phonetype.Domain.ValueObjects;

namespace AirTicketSystem.modules.phonetype.Application.UseCases;

public sealed class UpdatePhoneTypeUseCase
{
    private readonly IPhoneTypeRepository _repository;

    public UpdatePhoneTypeUseCase(IPhoneTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<PhoneType> ExecuteAsync(
        int id,
        string descripcion,
        CancellationToken cancellationToken = default)
    {
        var phoneType = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de teléfono con ID {id}.");

        var descripcionVO = DescripcionPhoneType.Crear(descripcion);

        if (!string.Equals(descripcionVO.Valor, phoneType.Descripcion.Valor, StringComparison.OrdinalIgnoreCase) &&
            await _repository.ExistsByDescripcionAsync(descripcionVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe otro tipo de teléfono con la descripción '{descripcionVO.Valor}'.");

        phoneType.ActualizarDescripcion(descripcionVO.Valor);
        await _repository.UpdateAsync(phoneType);
        return phoneType;
    }
}
