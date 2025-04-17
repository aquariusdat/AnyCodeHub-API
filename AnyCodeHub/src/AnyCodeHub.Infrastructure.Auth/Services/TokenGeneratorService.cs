using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AnyCodeHub.Domain.Entities.Identity;
using AnyCodeHub.Infrastructure.Auth.Abstractions;
using AnyCodeHub.Infrastructure.Auth.DependencyInjections.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace AnyCodeHub.Infrastructure.Auth.Services;
public class TokenGeneratorService : ITokenGeneratorService
{
    private readonly JwtOptions _jwtOptions;
    //private readonly IRepositoryBase<Domain.Entities.Identity.AppUser, Guid> _repositoryBase;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMemoryCache _memoryCache;
    private readonly string _invalidRefreshTokenCache = "INVALID_REFRESHTOKEN";
    private readonly EncryptOptions _encryptOptions;

    public TokenGeneratorService(JwtOptions jwtOptions, UserManager<AppUser> userManager, IMemoryCache memoryCache, EncryptOptions encryptOptions)
    {
        _jwtOptions = jwtOptions;
        _memoryCache = memoryCache;
        _encryptOptions = encryptOptions;
        _userManager = userManager;
    }

    public async Task<(string AccessToken, DateTime ExpireTime)> GenerateAccessToken(IEnumerable<Claim> claims)
    {
        try
        {
            #region Signing without RSA
            var keyBytes = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);
            var symmetricKey = new SymmetricSecurityKey(keyBytes);
            var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
            #endregion

            #region Signing with RSA
            //RsaSecurityKey rsaSecurityKey = GetRSAKey();
            //var signingCred = new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256);
            #endregion

            DateTime expireTime = DateTime.Now.AddMinutes(_jwtOptions.ExpirationMinutes);
            var token = new JwtSecurityToken(
                    issuer: _jwtOptions.Issuer,
                    audience: _jwtOptions.Audience,
                    signingCredentials: signingCredentials, // Signing without RSA
                                                            //signingCredentials: signingCred,
                    claims: claims,
                    expires: expireTime
                );
            return (AccessToken: new JwtSecurityTokenHandler().WriteToken(token), ExpireTime: expireTime);

            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme),
            //    Expires = expireTime,
            //    SigningCredentials = signingCredentials,
            //    Audience = _jwtOptions.Audience,
            //    Issuer = _jwtOptions.Issuer,
            //};

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            //return (AccessToken: token, ExpireTime: expireTime);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private RsaSecurityKey GetRSAKey()
    {
        var rsaKey = RSA.Create();
        string privateKey = File.ReadAllText(_jwtOptions.PrivateKeyPath);
        rsaKey.FromXmlString(privateKey);
        var rsaSecurityKey = new RsaSecurityKey(rsaKey);
        return rsaSecurityKey;
    }

    //public async Task<(string RefreshToken, DateTime ExpireTime)> GenerateRefreshToken(string UserName)
    //{
    //    try
    //    {
    //        var authenticatedEncryptionService = new AuthenticatedEncryptionService(Encoding.UTF8.GetBytes(_encryptOptions.EncryptAesKey));
    //        return (RefreshToken: authenticatedEncryptionService.Encrypt($"{UserName}"), ExpireTime: DateTime.Now.AddDays(_jwtOptions.RefreshTokenValidityInDays));
    //    }
    //    catch (Exception ex)
    //    {
    //        throw;
    //    }
    //}

    public async Task<(string RefreshToken, DateTime ExpireTime)> GenerateRefreshToken(IEnumerable<Claim> claims)
    {
        //var randomNumber = new byte[32];
        //using (var rng = RandomNumberGenerator.Create())
        //{
        //    rng.GetBytes(randomNumber);
        //    return (RefreshToken: Convert.ToBase64String(randomNumber), ExpireTime: DateTime.Now.AddDays(_jwtOptions.RefreshTokenValidityInDays));
        //}

        #region Signing without RSA
        var keyBytes = Encoding.UTF8.GetBytes(_jwtOptions.SecretRefreshKey);
        var symmetricKey = new SymmetricSecurityKey(keyBytes);
        var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
        #endregion

        #region Signing with RSA
        //RsaSecurityKey rsaSecurityKey = GetRSAKey();
        //var signingCred = new SigningCredentials(rsaSecurityKey, SecurityAlgorithms.RsaSha256);
        #endregion

        DateTime expireTime = DateTime.Now.AddDays(_jwtOptions.RefreshTokenValidityInDays);
        var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                signingCredentials: signingCredentials, // Signing without RSA
                                                        //signingCredentials: signingCred,
                claims: claims,
                expires: expireTime
            );
        return (AccessToken: new JwtSecurityTokenHandler().WriteToken(token), ExpireTime: expireTime);
    }

    public async Task<ClaimsPrincipal?> GetPrincipalFromExpiredToken(string token)
    {
        var secretKeys = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);


        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            IssuerSigningKey = new SymmetricSecurityKey(secretKeys),
            ClockSkew = TimeSpan.Zero,
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}
