using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JewelryAPI.Application.Interfaces;

namespace JewelryAPI.Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("daily/{date}")]
    public async Task<IActionResult> GetDailyReport(DateTime date)
    {
        return Ok(await _reportService.GetDailyReportAsync(date));
    }

    [HttpGet("monthly/{year}/{month}")]
    public async Task<IActionResult> GetMonthlyReport(int year, int month)
    {
        return Ok(await _reportService.GetMonthlyReportAsync(year, month));
    }
}
