using JewelryAPI.Application.DTOs;

namespace JewelryAPI.Application.Interfaces;

public interface IPurchaseService
{
    Task<PagedResult<PurchaseDto>> GetPurchasesPagedAsync(int page, int pageSize, string searchTerm);
    Task<IReadOnlyList<PurchaseDto>> GetAllPurchasesAsync();
    Task<IReadOnlyList<PurchaseDto>> GetPurchasesByCustomerAsync(Guid customerId);
    Task<PurchaseDto?> GetPurchaseByIdAsync(Guid id);
    Task<PurchaseDto> AddPurchaseAsync(CreatePurchaseDto dto);
    Task<bool> UpdatePurchaseAsync(UpdatePurchaseDto dto);
    Task<bool> DeletePurchaseAsync(Guid id);
}
