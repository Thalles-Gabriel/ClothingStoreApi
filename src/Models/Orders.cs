using System.ComponentModel.DataAnnotations;

namespace ClothingStore.API.Models;

public class Orders
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public DateTime TimeOfSale { get; set; } = DateTime.Now;

    [Required]
    public DateTime LastChanged { get; set; }

    public Payment PayMethod { get; set; } = Payment.Boleto;

    [Required]
    public string OrderAddress { get; set; } = null!;

    [Required]
    public Guid User_Id { get; set; }
}

public class InsertUpdateOrderView
{
    [Key]
    public Guid Id { get; set; }
    public Payment PayMethod { get; set; } = Payment.Boleto;
    [Required]
    public string? OrderAddress { get; set; }

    [Required]
    public Guid User_Id { get; set; }
}
