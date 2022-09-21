using System.ComponentModel.DataAnnotations;

namespace PayCoreFinalProject.Dto;

public class UserRegisterDto
{
    //required fields and some rules for registration
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Surname is required")]
    public string Surname { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(maximumLength: 20, MinimumLength = 8,
        ErrorMessage = "Password must contain minimum 8. character and maximum 20 characters.")]
    public string Password { get; set; }

    [Required(ErrorMessage = $"E-Mail is required")]
    [EmailAddress]
    public string Email { get; set; }
}