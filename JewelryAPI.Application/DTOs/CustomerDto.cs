using System.ComponentModel.DataAnnotations;

namespace JewelryAPI.Application.DTOs;

public class CustomerDto
{
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}

public class CreateCustomerDto
{
    [Required]
    [MaxLength(100)]
    public string CustomerName { get; set; } = string.Empty;
    [Required]
    [Phone]
    [MaxLength(20)]
    public string MobileNumber { get; set; } = string.Empty;
    [MaxLength(250)]
    public string Address { get; set; } = string.Empty;
}

public class UpdateCustomerDto
{
    [Required]
    public Guid CustomerId { get; set; }
    [Required]
    [MaxLength(100)]
    public string CustomerName { get; set; } = string.Empty;
    [Required]
    [Phone]
    [MaxLength(20)]
    public string MobileNumber { get; set; } = string.Empty;
    [MaxLength(250)]
    public string Address { get; set; } = string.Empty;
}
