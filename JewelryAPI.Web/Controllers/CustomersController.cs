using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JewelryAPI.Application.DTOs;
using JewelryAPI.Application.Interfaces;

namespace JewelryAPI.Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchTerm = "")
    {
        return Ok(await _customerService.GetCustomersPagedAsync(page, pageSize, searchTerm));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
    {
        var result = await _customerService.AddCustomerAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.CustomerId }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCustomerDto dto)
    {
        if (id != dto.CustomerId) return BadRequest();
        var success = await _customerService.UpdateCustomerAsync(dto);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _customerService.DeleteCustomerAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}
