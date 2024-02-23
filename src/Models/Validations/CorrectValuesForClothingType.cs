using System.ComponentModel.DataAnnotations;
using ClothingStore.API.Models;

namespace ClothingStore.API;

public class CorrectGenderForClothingType : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var clothing = validationContext.ObjectInstance as Clothes;

        switch (clothing.Type)
        {
            case Types.Tenis:
                if(clothing.Gender != Genders.Infantil && Convert.ToInt32(clothing.Size) > 36) 
                    return new ValidationResult("Tênis infantil com numeração acima de 36 passa a ser adulto.");
                if(clothing.Gender == Genders.Feminino && Convert.ToInt32(clothing.Size) > 42) 
                    return new ValidationResult("Tênis feminino com numeração acima de 42 se encaixa no masculino.");
                break;

            case Types.Saia:
                if(clothing.Gender == Genders.Masculino)
                    return new ValidationResult("Saias masculinas se encaixam em calças.");
                break;
        }

        return ValidationResult.Success;
    }
}
