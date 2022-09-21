using System.ComponentModel.DataAnnotations;

namespace PayCoreFinalProject.Base.Token;

public class TokenRequest
{
    //required, email attribute
    [Required(ErrorMessage = "Email can not be empty.")]
    [EmailAddress]
    public string EMail { get; set; }

    [Required(ErrorMessage = "Password can not be empty.")]
    public string Password { get; set; }
}