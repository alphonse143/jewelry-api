using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.Logging;
using JewelryAPI.Application.DTOs;
using JewelryAPI.Application.Interfaces;
using JewelryAPI.Core.Entities;
using JewelryAPI.Core.Interfaces;

namespace JewelryAPI.Application.Services;

public class PurchaseService : IPurchaseService
{
    private readonly IRepository<Purchase> _purchaseRepo;
    private readonly IRepository<Customer> _customerRepo;
    private readonly IMapper _mapper;
    private readonly ILogger<PurchaseService> _logger;

    public PurchaseService(IRepository<Purchase> purchaseRepo, IRepository<Customer> customerRepo, IMapper mapper, ILogger<PurchaseService> logger)
    {
        _purchaseRepo = purchaseRepo;
        _customerRepo = customerRepo;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResult<PurchaseDto>> GetPurchasesPagedAsync(int page, int pageSize, string searchTerm)
    {
        var query = _purchaseRepo.GetQueryable().AsNoTracking().Include(p => p.Customer).AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var lowerSearch = searchTerm.ToLower();
            query = query.Where(p => p.ItemName.ToLower().Contains(lowerSearch) || 
                                     (p.Customer != null && p.Customer.CustomerName.ToLower().Contains(lowerSearch)));
        }

        var totalCount = await query.CountAsync();
        var items = await query.OrderByDescending(p => p.PurchaseDate)
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

        return new PagedResult<PurchaseDto>
        {
            Items = _mapper.Map<IEnumerable<PurchaseDto>>(items),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<IReadOnlyList<PurchaseDto>> GetAllPurchasesAsync()
    {
        var purchases = await _purchaseRepo.GetAllAsync();
        foreach(var p in purchases) {
            p.Customer = await _customerRepo.GetByIdAsync(p.CustomerId);
        }
        return _mapper.Map<IReadOnlyList<PurchaseDto>>(purchases);
    }

    public async Task<IReadOnlyList<PurchaseDto>> GetPurchasesByCustomerAsync(Guid customerId)
    {
        var purchases = await _purchaseRepo.GetAllAsync();
        var customerPurchases = purchases.Where(p => p.CustomerId == customerId).ToList();
        return _mapper.Map<IReadOnlyList<PurchaseDto>>(customerPurchases);
    }

    public async Task<PurchaseDto?> GetPurchaseByIdAsync(Guid id)
    {
        var purchase = await _purchaseRepo.GetByIdAsync(id);
        if(purchase != null) {
            purchase.Customer = await _customerRepo.GetByIdAsync(purchase.CustomerId);
        }
        return purchase == null ? null : _mapper.Map<PurchaseDto>(purchase);
    }

    public async Task<PurchaseDto> AddPurchaseAsync(CreatePurchaseDto dto)
    {
        var purchase = _mapper.Map<Purchase>(dto);
        var added = await _purchaseRepo.AddAsync(purchase);
        return _mapper.Map<PurchaseDto>(added);
    }

    public async Task<bool> UpdatePurchaseAsync(UpdatePurchaseDto dto)
    {
        var purchase = await _purchaseRepo.GetByIdAsync(dto.PurchaseId);
        if (purchase == null) return false;

        _mapper.Map(dto, purchase);
        await _purchaseRepo.UpdateAsync(purchase);
        return true;
    }

    public async Task<bool> DeletePurchaseAsync(Guid id)
    {
        var purchase = await _purchaseRepo.GetByIdAsync(id);
        if (purchase == null) return false;

        await _purchaseRepo.DeleteAsync(purchase);
        return true;
    }
}
