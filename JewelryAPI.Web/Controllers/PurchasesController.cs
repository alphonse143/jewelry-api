using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JewelryAPI.Application.DTOs;
using JewelryAPI.Application.Interfaces;

namespace JewelryAPI.Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PurchasesController : ControllerBase
{
    private readonly IPurchaseService _purchaseService;

    public PurchasesController(IPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchTerm = "")
    {
        return Ok(await _purchaseService.GetPurchasesPagedAsync(page, pageSize, searchTerm));
    }

    [HttpGet("customer/{customerId:guid}")]
    public async Task<IActionResult> GetByCustomer(Guid customerId)
    {
        return Ok(await _purchaseService.GetPurchasesByCustomerAsync(customerId));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var purchase = await _purchaseService.GetPurchaseByIdAsync(id);
        if (purchase == null) return NotFound();
        return Ok(purchase);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePurchaseDto dto)
    {
        var result = await _purchaseService.AddPurchaseAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.PurchaseId }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePurchaseDto dto)
    {
        if (id != dto.PurchaseId) return BadRequest();
        var success = await _purchaseService.UpdatePurchaseAsync(dto);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var success = await _purchaseService.DeletePurchaseAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}
