// src/modules/luggage/Application/Services/LuggageService.cs
using AirTicketSystem.modules.luggage.Application.Interfaces;
using AirTicketSystem.modules.luggage.Application.UseCases;
using AirTicketSystem.modules.luggage.Domain.aggregate;

namespace AirTicketSystem.modules.luggage.Application.Services;

public sealed class LuggageService : ILuggageService
{
    private readonly RegisterLuggageUseCase         _register;
    private readonly GetLuggageByPassengerUseCase   _getByPassenger;
    private readonly ProcessCheckInLuggageUseCase   _processCheckIn;
    private readonly SendToBaggageUseCase           _sendToBaggage;
    private readonly ReportLostLuggageUseCase       _reportLost;
    private readonly ReportDamagedLuggageUseCase    _reportDamaged;

    public LuggageService(
        RegisterLuggageUseCase       register,
        GetLuggageByPassengerUseCase getByPassenger,
        ProcessCheckInLuggageUseCase processCheckIn,
        SendToBaggageUseCase         sendToBaggage,
        ReportLostLuggageUseCase     reportLost,
        ReportDamagedLuggageUseCase  reportDamaged)
    {
        _register       = register;
        _getByPassenger = getByPassenger;
        _processCheckIn = processCheckIn;
        _sendToBaggage  = sendToBaggage;
        _reportLost     = reportLost;
        _reportDamaged  = reportDamaged;
    }

    public Task<Luggage> RegisterAsync(
        int pasajeroReservaId, int vueloId, int tipoEquipajeId,
        string? descripcion, decimal? pesoDeclaradoKg,
        int? largoDeclaradoCm, int? anchoDeclaradoCm, int? altoDeclaradoCm)
        => _register.ExecuteAsync(
            pasajeroReservaId, vueloId, tipoEquipajeId,
            descripcion, pesoDeclaradoKg,
            largoDeclaradoCm, anchoDeclaradoCm, altoDeclaradoCm);

    public Task<IReadOnlyCollection<Luggage>> GetByPassengerAsync(int pasajeroReservaId)
        => _getByPassenger.ExecuteAsync(pasajeroReservaId);

    public Task<Luggage> ProcessCheckInAsync(
        int id, decimal pesoRealKg, decimal pesoMaximoPermitido,
        decimal costoPorKgExcedido, int? largoRealCm, int? anchoRealCm, int? altoRealCm)
        => _processCheckIn.ExecuteAsync(
            id, pesoRealKg, pesoMaximoPermitido,
            costoPorKgExcedido, largoRealCm, anchoRealCm, altoRealCm);

    public Task<Luggage> SendToBaggageAsync(int id)
        => _sendToBaggage.ExecuteAsync(id);

    public Task<Luggage> ReportLostAsync(int id)
        => _reportLost.ExecuteAsync(id);

    public Task<Luggage> ReportDamagedAsync(int id)
        => _reportDamaged.ExecuteAsync(id);
}
