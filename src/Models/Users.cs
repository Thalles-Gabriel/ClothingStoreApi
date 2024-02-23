using System.ComponentModel.DataAnnotations;

namespace ClothingStore.API.Models;

public class Users
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Nome inválido.")]
    public string Name { get; set; } = "";

    [Required]
    [EmailAddress(ErrorMessage = "Email inválido.")]
    public string Email { get; set; } = "";

    [Required]
    [MinLength(8, ErrorMessage = "A senha precisa ter ao menos 8 caracteres.")]
    [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[$*&@#])[0-9a-zA-Z$*&@#]{8,}$", 
            ErrorMessage = "A senha precisa ter números, símbolos, letras maiúsculas e minúsculas.")]
    public string Password { get; set; } = "";

    [StringLength(50, MinimumLength = 5)]
    public string Address { get; set; } = "";
}
