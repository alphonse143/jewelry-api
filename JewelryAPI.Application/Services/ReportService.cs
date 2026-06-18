using JewelryAPI.Application.Interfaces;
using JewelryAPI.Core.Entities;
using JewelryAPI.Core.Interfaces;

namespace JewelryAPI.Application.Services;

public class ReportService : IReportService
{
    private readonly IRepository<Purchase> _purchaseRepo;

    public ReportService(IRepository<Purchase> purchaseRepo)
    {
        _purchaseRepo = purchaseRepo;
    }

    public async Task<object> GetDailyReportAsync(DateTime date)
    {
        var allPurchases = await _purchaseRepo.GetAllAsync();
        var dailyPurchases = allPurchases.Where(p => p.PurchaseDate.Date == date.Date).ToList();

        return new
        {
            Date = date.ToString("yyyy-MM-dd"),
            TotalSales = dailyPurchases.Sum(p => p.Amount),
            TotalWeight = dailyPurchases.Sum(p => p.Weight),
            TransactionCount = dailyPurchases.Count
        };
    }

    public async Task<object> GetMonthlyReportAsync(int year, int month)
    {
        var allPurchases = await _purchaseRepo.GetAllAsync();
        var monthlyPurchases = allPurchases.Where(p => p.PurchaseDate.Year == year && p.PurchaseDate.Month == month).ToList();

        return new
        {
            Year = year,
            Month = month,
            TotalSales = monthlyPurchases.Sum(p => p.Amount),
            TotalWeight = monthlyPurchases.Sum(p => p.Weight),
            TransactionCount = monthlyPurchases.Count
        };
    }
}
