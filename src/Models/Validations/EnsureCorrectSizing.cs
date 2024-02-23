using System.ComponentModel.DataAnnotations;

namespace ClothingStore.API.Models.Validations;

public class EnsureCorrectSizing : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var clothing = validationContext.ObjectInstance as Clothes;
        if (clothing is null)
            return new ValidationResult("Houve um erro na validação.");

        var isNumber = int.TryParse(clothing.Size, out var sizeNumber);

        switch (clothing.Type)
        {
            case Types.Camisa:
            case Types.Saia:
                if (isNumber)
                    return new ValidationResult("O tamanho só pode ser composto por letras para este tipo de roupa.");

                break;
            case Types.Tenis:
                if (clothing.Gender != Genders.Infantil && sizeNumber > 36)
                    return new ValidationResult("Tênis infantil com numeração acima de 36 passa a ser adulto.");
                if (clothing.Gender == Genders.Feminino && sizeNumber > 42)
                    return new ValidationResult("Tênis feminino com numeração acima de 42 se encaixa no masculino.");
                break;
            default:
                if (!isNumber)
                    return new ValidationResult("O tamanho precisa ser numérico para este tipo de roupa.");
                break;
        }

        return ValidationResult.Success;
    }
}
