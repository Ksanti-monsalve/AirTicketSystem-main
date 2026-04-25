// src/modules/airline/Application/UseCases/AddAirlinePhoneUseCase.cs
using AirTicketSystem.modules.airline.Domain.aggregate;
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airline.Domain.ValueObjects;
using AirTicketSystem.modules.phonetype.Domain.Repositories;

namespace AirTicketSystem.modules.airline.Application.UseCases;

public sealed class AddAirlinePhoneUseCase
{
    private readonly IAirlineRepository      _airlineRepository;
    private readonly IAirlinePhoneRepository _phoneRepository;
    private readonly IPhoneTypeRepository    _phoneTypeRepository;

    public AddAirlinePhoneUseCase(
        IAirlineRepository      airlineRepository,
        IAirlinePhoneRepository phoneRepository,
        IPhoneTypeRepository    phoneTypeRepository)
    {
        _airlineRepository   = airlineRepository;
        _phoneRepository     = phoneRepository;
        _phoneTypeRepository = phoneTypeRepository;
    }

    public async Task<AirlinePhone> ExecuteAsync(
        int aerolineaId,
        int tipoTelefonoId,
        string numero,
        string? indicativo,
        bool esPrincipal,
        CancellationToken cancellationToken = default)
    {
        _ = await _airlineRepository.FindByIdAsync(aerolineaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una aerolínea con ID {aerolineaId}.");

        _ = await _phoneTypeRepository.FindByIdAsync(tipoTelefonoId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de teléfono con ID {tipoTelefonoId}.");

        var numeroVO = NumeroAirlinePhone.Crear(numero);

        if (await _phoneRepository.ExistsByNumeroAndAerolineaAsync(
            numeroVO.Valor, aerolineaId))
            throw new InvalidOperationException(
                $"El número '{numeroVO.Valor}' ya está registrado para esta aerolínea.");

        if (esPrincipal)
            await _phoneRepository.DesmarcarPrincipalByAerolineaAsync(aerolineaId);

        var airlinePhone = AirlinePhone.Crear(
            aerolineaId, tipoTelefonoId, numero, indicativo, esPrincipal);

        await _phoneRepository.SaveAsync(airlinePhone);
        return airlinePhone;
    }
}
