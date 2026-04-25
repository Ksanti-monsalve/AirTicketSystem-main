// src/modules/additionalcharge/Application/Services/AdditionalChargeService.cs
using AirTicketSystem.modules.additionalcharge.Application.Interfaces;
using AirTicketSystem.modules.additionalcharge.Application.UseCases;
using AirTicketSystem.modules.additionalcharge.Domain.aggregate;

namespace AirTicketSystem.modules.additionalcharge.Application.Services;

public sealed class AdditionalChargeService : IAdditionalChargeService
{
    private readonly CreateAdditionalChargeUseCase _create;
    private readonly GetAdditionalChargeByIdUseCase _getById;
    private readonly GetAllAdditionalChargesUseCase _getAll;
    private readonly GetChargesByBookingUseCase _getByBooking;
    private readonly GetTotalChargesByBookingUseCase _getTotal;
    private readonly DeleteAdditionalChargeUseCase _delete;

    public AdditionalChargeService(
        CreateAdditionalChargeUseCase create,
        GetAdditionalChargeByIdUseCase getById,
        GetAllAdditionalChargesUseCase getAll,
        GetChargesByBookingUseCase getByBooking,
        GetTotalChargesByBookingUseCase getTotal,
        DeleteAdditionalChargeUseCase delete)
    {
        _create      = create;
        _getById     = getById;
        _getAll      = getAll;
        _getByBooking = getByBooking;
        _getTotal    = getTotal;
        _delete      = delete;
    }

    public Task<AdditionalCharge> CreateAsync(int reservaId, string concepto, decimal monto)
        => _create.ExecuteAsync(reservaId, concepto, monto);

    public Task<AdditionalCharge> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<AdditionalCharge>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<IReadOnlyCollection<AdditionalCharge>> GetByBookingAsync(int reservaId)
        => _getByBooking.ExecuteAsync(reservaId);

    public Task<decimal> GetTotalByBookingAsync(int reservaId)
        => _getTotal.ExecuteAsync(reservaId);

    public Task DeleteAsync(int id) => _delete.ExecuteAsync(id);
}
