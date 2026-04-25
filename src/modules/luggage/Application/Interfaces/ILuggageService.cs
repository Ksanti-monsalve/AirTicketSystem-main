// src/modules/luggage/Application/Interfaces/ILuggageService.cs
using AirTicketSystem.modules.luggage.Domain.aggregate;

namespace AirTicketSystem.modules.luggage.Application.Interfaces;

public interface ILuggageService
{
    Task<Luggage> RegisterAsync(
        int pasajeroReservaId,
        int vueloId,
        int tipoEquipajeId,
        string? descripcion,
        decimal? pesoDeclaradoKg,
        int? largoDeclaradoCm,
        int? anchoDeclaradoCm,
        int? altoDeclaradoCm);
    Task<IReadOnlyCollection<Luggage>> GetByPassengerAsync(int pasajeroReservaId);
    Task<Luggage> ProcessCheckInAsync(
        int id,
        decimal pesoRealKg,
        decimal pesoMaximoPermitido,
        decimal costoPorKgExcedido,
        int? largoRealCm,
        int? anchoRealCm,
        int? altoRealCm);
    Task<Luggage> SendToBaggageAsync(int id);
    Task<Luggage> ReportLostAsync(int id);
    Task<Luggage> ReportDamagedAsync(int id);
}
