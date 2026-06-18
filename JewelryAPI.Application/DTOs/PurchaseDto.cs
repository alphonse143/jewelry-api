using System.ComponentModel.DataAnnotations;

namespace JewelryAPI.Application.DTOs;

public class PurchaseDto
{
    public Guid PurchaseId { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public decimal Weight { get; set; }
    public decimal Amount { get; set; }
    public DateTime PurchaseDate { get; set; }
}

public class CreatePurchaseDto
{
    [Required]
    public Guid CustomerId { get; set; }
    [Required]
    [MaxLength(150)]
    public string ItemName { get; set; } = string.Empty;
    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;
    [Range(0.01, 10000)]
    public decimal Weight { get; set; }
    [Range(0.01, 1000000)]
    public decimal Amount { get; set; }
    [Required]
    public DateTime PurchaseDate { get; set; }
}

public class UpdatePurchaseDto
{
    [Required]
    public Guid PurchaseId { get; set; }
    [Required]
    [MaxLength(150)]
    public string ItemName { get; set; } = string.Empty;
    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;
    [Range(0.01, 10000)]
    public decimal Weight { get; set; }
    [Range(0.01, 1000000)]
    public decimal Amount { get; set; }
    [Required]
    public DateTime PurchaseDate { get; set; }
}
