using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using Intermediate.Data;
using Intermediate.Dtos;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace Intermediate.Helpers;

public class AuthHelper
{
    private readonly IConfiguration _configuration;

    private readonly DataContextDapper _dapper;


    public AuthHelper(IConfiguration configuration)
    {
        _configuration = configuration;
        _dapper = new DataContextDapper(_configuration);
    }

    public byte[] GetPasswordHash(string password, byte[] passwordSalt)
    {
        var passwordSaltPlusString = _configuration.GetSection("AppSettings:PasswordKey").Value + Convert.ToBase64String(passwordSalt);

        return KeyDerivation.Pbkdf2(password, Encoding.ASCII.GetBytes(passwordSaltPlusString), KeyDerivationPrf.HMACSHA256, 100000, 256 / 8);
    }

    public static bool ConstantTimeComparison(byte[] a, byte[] b)
    {
        if (a.Length != b.Length)
            return false;

        var result = 0;
        for (var i = 0; i < a.Length; i++) result |= a[i] ^ b[i];

        return result == 0;
    }

    public string CreateToken(int userId)
    {
        Claim[] claims = new Claim[]
        {
            new("userId", userId.ToString())
        };

        var tokenKeyString = _configuration.GetSection("AppSettings:TokenKey").Value;

        var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKeyString != null ? tokenKeyString : ""));

        var credentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);

        var descriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = credentials,
            Expires = DateTime.Now.AddDays(1)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(descriptor);

        return tokenHandler.WriteToken(token);
    }

    public bool SetPassword(UserForLoginDto userForSetPassword)
    {
        var passwordSalt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetNonZeroBytes(passwordSalt);
        }

        var passwordHash = GetPasswordHash(userForSetPassword.Password, passwordSalt);

        const string sqlAddAuth = @"EXEC TutorialAppSchema.spRegistration_Upsert
                @Email = @EmailParam, 
                @PasswordHash = @PasswordHashParam, 
                @PasswordSalt = @PasswordSaltParam";

        var sqlParameters = new DynamicParameters();
        sqlParameters.Add("@EmailParam", userForSetPassword.Email, DbType.String);
        sqlParameters.Add("@PasswordHashParam", passwordHash, DbType.Binary);
        sqlParameters.Add("@PasswordSaltParam", passwordSalt, DbType.Binary);

        return _dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters);
    }
}