using System.ComponentModel.DataAnnotations;

namespace PayCoreFinalProject.Base.Token;

public class TokenResponse
{
    [Display(Name = "Expire Time")] public DateTime ExpireTime { get; set; }
    [Display(Name="Access Token")] public string AccessToken { get; set; }

    public string Name { get; set; }
    public string Surname { get; set; }
    public int SessionTimeInSecond { get; set; }
    
}