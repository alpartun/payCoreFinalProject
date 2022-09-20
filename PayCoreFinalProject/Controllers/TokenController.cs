using Microsoft.AspNetCore.Mvc;
using PayCoreFinalProject.Base.Response;
using PayCoreFinalProject.Base.Token;
using PayCoreFinalProject.Service.Token.Abstract;

namespace PayCoreFinalProject.Controllers;
[ApiController]
[Route("[controller]")]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;
    // token service injection
    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("Login")]
    
    public BaseResponse<TokenResponse> Login([FromBody] TokenRequest tokenRequest)
    {
        //call tokenservice and generate method
        var response = _tokenService.GenerateToken(tokenRequest);
        return response;
    }

}