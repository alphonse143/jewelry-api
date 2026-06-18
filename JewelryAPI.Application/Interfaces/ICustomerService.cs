using JewelryAPI.Application.DTOs;

namespace JewelryAPI.Application.Interfaces;

public interface ICustomerService
{
    Task<PagedResult<CustomerDto>> GetCustomersPagedAsync(int page, int pageSize, string searchTerm);
    Task<IReadOnlyList<CustomerDto>> GetAllCustomersAsync();
    Task<CustomerDto?> GetCustomerByIdAsync(Guid id);
    Task<CustomerDto> AddCustomerAsync(CreateCustomerDto dto);
    Task<bool> UpdateCustomerAsync(UpdateCustomerDto dto);
    Task<bool> DeleteCustomerAsync(Guid id);
}
