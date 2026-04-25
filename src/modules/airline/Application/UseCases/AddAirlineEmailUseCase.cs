// src/modules/airline/Application/UseCases/AddAirlineEmailUseCase.cs
using AirTicketSystem.modules.airline.Domain.aggregate;
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airline.Domain.ValueObjects;
using AirTicketSystem.modules.emailtype.Domain.Repositories;

namespace AirTicketSystem.modules.airline.Application.UseCases;

public sealed class AddAirlineEmailUseCase
{
    private readonly IAirlineRepository     _airlineRepository;
    private readonly IAirlineEmailRepository _emailRepository;
    private readonly IEmailTypeRepository   _emailTypeRepository;

    public AddAirlineEmailUseCase(
        IAirlineRepository     airlineRepository,
        IAirlineEmailRepository emailRepository,
        IEmailTypeRepository   emailTypeRepository)
    {
        _airlineRepository   = airlineRepository;
        _emailRepository     = emailRepository;
        _emailTypeRepository = emailTypeRepository;
    }

    public async Task<AirlineEmail> ExecuteAsync(
        int aerolineaId,
        int tipoEmailId,
        string email,
        bool esPrincipal,
        CancellationToken cancellationToken = default)
    {
        _ = await _airlineRepository.FindByIdAsync(aerolineaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una aerolínea con ID {aerolineaId}.");

        _ = await _emailTypeRepository.FindByIdAsync(tipoEmailId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de email con ID {tipoEmailId}.");

        var emailVO = EmailAirlineEmail.Crear(email);

        if (await _emailRepository.ExistsByEmailAndAerolineaAsync(
            emailVO.Valor, aerolineaId))
            throw new InvalidOperationException(
                $"El email '{emailVO.Valor}' ya está registrado para esta aerolínea.");

        if (esPrincipal)
            await _emailRepository.DesmarcarPrincipalByAerolineaAsync(aerolineaId);

        var airlineEmail = AirlineEmail.Crear(aerolineaId, tipoEmailId, email, esPrincipal);

        await _emailRepository.SaveAsync(airlineEmail);
        return airlineEmail;
    }
}
