using System.ComponentModel.DataAnnotations;

namespace ClothingStore.API.Models;

public class OrderClothes
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [Range(1, 10000)]
    public int TotalClothingQuantity { get; set; }

    [Required]
    public Guid Clothes_Id { get; set; }

    [Required]
    public Guid Orders_Id { get; set; }
}
