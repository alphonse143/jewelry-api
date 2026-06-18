using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JewelryAPI.Core.Enums;

namespace JewelryAPI.Core.Entities;

public class Purchase
{
    [Key]
    public Guid PurchaseId { get; set; } = Guid.NewGuid();

    [Required]
    public Guid CustomerId { get; set; }
    
    [ForeignKey(nameof(CustomerId))]
    public Customer? Customer { get; set; }

    [Required]
    public string ItemName { get; set; } = string.Empty;

    [Required]
    public string Category { get; set; } = CategoryEnum.Gold.ToString();

    [Column(TypeName = "decimal(18,2)")]
    public decimal Weight { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
}
