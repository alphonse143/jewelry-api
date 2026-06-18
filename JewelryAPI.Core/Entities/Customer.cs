using System.ComponentModel.DataAnnotations;

namespace JewelryAPI.Core.Entities;

public class Customer
{
    [Key]
    public Guid CustomerId { get; set; } = Guid.NewGuid();

    [Required]
    public string CustomerName { get; set; } = string.Empty;

    [Required]
    public string MobileNumber { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
}
