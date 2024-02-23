using System.ComponentModel.DataAnnotations;
using ClothingStore.API.Models.Validations;

namespace ClothingStore.API.Models;

public class Clothes
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [EnsureCorrectSizing]
    [StringLength(4, MinimumLength = 1)]
    public string Size { get; set; } = "";

    [Required]
    [RegularExpression(@"^[a-zA-Z ]*$", 
                        ErrorMessage = "Valor do nome inválido.")]
    [StringLength(50, MinimumLength = 3)]
    public string Name { get; set; } = "";

    [StringLength(10)]
    public string Color { get; set; } = "";

    [StringLength(15)]
    [Required]
    public string? Brand { get; set; }

    [Required]
    [Range(0, 100000)]
    [RegularExpression(@"(^[0-9]*\.[0-9]{2}$)")]
    public decimal Price { get; set; } = 0;

    public string[]? Tags { get; set; }

    [StringLength(100)]
    public string? Description { get; set; }

    [CorrectGenderForClothingType]
    public Genders Gender { get; set; } = Genders.Unisex;

    [Required]
    public Types Type { get; set; }

    [Required]
    [Range(0, 10000)]
    public int Quantity { get; set; } = 0;
}
