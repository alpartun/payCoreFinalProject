using PayCoreFinalProject.Base.Response;
using PayCoreFinalProject.Base.Token;

namespace PayCoreFinalProject.Service.Token.Abstract;

public interface ITokenService
{
    BaseResponse<TokenResponse> GenerateToken(TokenRequest tokenRequest);
}