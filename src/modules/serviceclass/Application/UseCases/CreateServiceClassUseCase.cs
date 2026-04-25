// src/modules/serviceclass/Application/UseCases/CreateServiceClassUseCase.cs
using AirTicketSystem.modules.serviceclass.Domain.Repositories;
using AirTicketSystem.modules.serviceclass.Domain.aggregate;
using AirTicketSystem.modules.serviceclass.Domain.ValueObjects;

namespace AirTicketSystem.modules.serviceclass.Application.UseCases;

public class CreateServiceClassUseCase
{
    private readonly IServiceClassRepository _repository;

    public CreateServiceClassUseCase(IServiceClassRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceClass> ExecuteAsync(
        string nombre,
        string codigo,
        string? descripcion,
        CancellationToken cancellationToken = default)
    {
        var nombreVO = NombreServiceClass.Crear(nombre);
        var codigoVO = CodigoServiceClass.Crear(codigo);

        if (await _repository.ExistsByCodigoAsync(codigoVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe una clase de servicio con el código '{codigoVO.Valor}'.");

        var serviceClass = ServiceClass.Crear(nombreVO.Valor, codigoVO.Valor, descripcion);
        await _repository.SaveAsync(serviceClass);
        return serviceClass;
    }
}