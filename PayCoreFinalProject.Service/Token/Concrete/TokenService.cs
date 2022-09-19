using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NHibernate;
using PayCoreFinalProject.Base.Jwt;
using PayCoreFinalProject.Base.Response;
using PayCoreFinalProject.Base.Token;
using PayCoreFinalProject.Data.Model;
using PayCoreFinalProject.Data.Repository;
using PayCoreFinalProject.Service.Token.Abstract;

namespace PayCoreFinalProject.Service.Token.Concrete;

public class TokenService : ITokenService
{
    protected readonly ISession _session;
    protected readonly IHibernateRepository<User> _hibernateRepository;
    private readonly JwtConfig _jwtConfig;

    public TokenService(ISession session,IOptionsMonitor<JwtConfig> jwtConfig)
    {
        _session = session;
        _jwtConfig = jwtConfig.CurrentValue;
        _hibernateRepository = new HibernateRepository<User>(session);
    }
    
    
    public BaseResponse<TokenResponse> GenerateToken(TokenRequest tokenRequest)
    {
        try
        {
            if (tokenRequest is null)
            {
                return new BaseResponse<TokenResponse>("Please enter valid information.");
            }

            var user = _hibernateRepository.Where(x => x.Email.Equals(tokenRequest.EMail)).FirstOrDefault();

            if (user is null)
            {
                return new BaseResponse<TokenResponse>("Please validate your information that you provided.");
            }

            if (!VerifyPasswordHash(tokenRequest.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new BaseResponse<TokenResponse>("Email or Password is false.");
            }

            DateTime now = DateTime.UtcNow;
            string token = GetToken(user, now);

            TokenResponse tokenResponse = new TokenResponse
            {
                AccessToken = token,
                ExpireTime = now.AddMinutes(_jwtConfig.AccessTokenExpiration),
                Name = user.Name,
                Surname = user.Surname,
                SessionTimeInSecond = _jwtConfig.AccessTokenExpiration*60,
            };

            return new BaseResponse<TokenResponse>(tokenResponse);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    private string GetToken(User user, DateTime now)
    {
        Claim[] claims = GetClaims(user);
        byte[] secret = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

        var shouldAddAudienceClaim =
            string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);
        var jwtToken = new JwtSecurityToken(
            _jwtConfig.Issuer,
            shouldAddAudienceClaim ? _jwtConfig.Audience : string.Empty,
            claims,
            expires: now.AddMinutes(_jwtConfig.AccessTokenExpiration),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secret),
                SecurityAlgorithms.HmacSha256Signature));

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        return accessToken;
    }

    private Claim[] GetClaims(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Surname, user.Surname),
            new Claim(ClaimTypes.Email, user.Email),



        };
        return claims;
    }

    private bool VerifyPasswordHash(string password, byte[] userPasswordHash, byte[] userPasswordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(userPasswordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != userPasswordHash[i])
                {
                    return false;
                }
            }

            return true;

        }
    }
}