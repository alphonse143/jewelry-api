using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.Logging;
using JewelryAPI.Application.DTOs;
using JewelryAPI.Application.Interfaces;
using JewelryAPI.Core.Entities;
using JewelryAPI.Core.Interfaces;

namespace JewelryAPI.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly IRepository<Customer> _customerRepo;
    private readonly IMapper _mapper;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(IRepository<Customer> customerRepo, IMapper mapper, ILogger<CustomerService> logger)
    {
        _customerRepo = customerRepo;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResult<CustomerDto>> GetCustomersPagedAsync(int page, int pageSize, string searchTerm)
    {
        var query = _customerRepo.GetQueryable().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var lowerSearch = searchTerm.ToLower();
            query = query.Where(c => c.CustomerName.ToLower().Contains(lowerSearch) || c.MobileNumber.Contains(lowerSearch));
        }

        var totalCount = await query.CountAsync();
        var items = await query.OrderByDescending(c => c.CreatedDate)
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

        return new PagedResult<CustomerDto>
        {
            Items = _mapper.Map<IEnumerable<CustomerDto>>(items),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<IReadOnlyList<CustomerDto>> GetAllCustomersAsync()
    {
        var customers = await _customerRepo.GetAllAsync();
        return _mapper.Map<IReadOnlyList<CustomerDto>>(customers);
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(Guid id)
    {
        var customer = await _customerRepo.GetByIdAsync(id);
        return customer == null ? null : _mapper.Map<CustomerDto>(customer);
    }

    public async Task<CustomerDto> AddCustomerAsync(CreateCustomerDto dto)
    {
        var customer = _mapper.Map<Customer>(dto);
        var added = await _customerRepo.AddAsync(customer);
        return _mapper.Map<CustomerDto>(added);
    }

    public async Task<bool> UpdateCustomerAsync(UpdateCustomerDto dto)
    {
        var customer = await _customerRepo.GetByIdAsync(dto.CustomerId);
        if (customer == null) return false;

        _mapper.Map(dto, customer);
        await _customerRepo.UpdateAsync(customer);
        return true;
    }

    public async Task<bool> DeleteCustomerAsync(Guid id)
    {
        var customer = await _customerRepo.GetByIdAsync(id);
        if (customer == null) return false;

        await _customerRepo.DeleteAsync(customer);
        return true;
    }
}
