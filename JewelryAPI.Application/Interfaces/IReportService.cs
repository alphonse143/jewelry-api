using JewelryAPI.Application.DTOs;

namespace JewelryAPI.Application.Interfaces;

public interface IReportService
{
    Task<object> GetDailyReportAsync(DateTime date);
    Task<object> GetMonthlyReportAsync(int year, int month);
}
