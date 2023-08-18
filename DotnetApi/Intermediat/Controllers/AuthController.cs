using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Intermediate.Data;
using Intermediate.Dtos;
using Intermediate.Helpers;
using Intermediate.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace Intermediate.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly DataContextDapper _dataContext;

    private readonly AuthHelper _authHelper;

    public AuthController(IConfiguration configuration)
    {
        _dataContext = new DataContextDapper(configuration);
        _authHelper = new AuthHelper(configuration);
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public IActionResult Register(UserForRegistrationDto userForRegistration)
    {
        if (userForRegistration.Password != userForRegistration.PasswordConfirm) throw new Exception("Passwords do not match!");

        var sqlCheckUserExists = "SELECT Email From TutorialAppSchema.Auth WHERE Email = '" + userForRegistration.Email + "'";
        var existingUsers = _dataContext.LoadData<string>(sqlCheckUserExists);

        if (existingUsers.Any()) throw new Exception("User with this email already exists!");

        var passwordSalt = new byte[16];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetNonZeroBytes(passwordSalt);
        }

        var passwordHash = _authHelper.GetPasswordHash(userForRegistration.Password, passwordSalt);

        var sqlAddAuth = @"INSERT INTO TutorialAppSchema.Auth ([Email], [PasswordHash], [PasswordSalt]) VALUES (@Email, @PasswordHash, @PasswordSalt)";

        var sqlParameters = new List<SqlParameter>();

        var emailParameter = new SqlParameter("@Email", SqlDbType.NVarChar)
        {
            Value = userForRegistration.Email
        };

        var passwordSaltParameter = new SqlParameter("@PasswordSalt", SqlDbType.VarBinary)
        {
            Value = passwordSalt
        };

        var passwordHashParameter = new SqlParameter("@PasswordHash", SqlDbType.VarBinary)
        {
            Value = passwordHash
        };

        sqlParameters.Add(emailParameter);
        sqlParameters.Add(passwordSaltParameter);
        sqlParameters.Add(passwordHashParameter);

        if (_dataContext.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
        {
            var sqlAddUser = @"
                            INSERT INTO TutorialAppSchema.Users(
                                [FirstName],
                                [LastName],
                                [Email],
                                [Gender],
                                [Active]
                            ) VALUES (" + "'" + userForRegistration.FirstName + "', '" + userForRegistration.LastName + "', '" + userForRegistration.Email + "', '" + userForRegistration.Gender + "', 1)";


            if (!_dataContext.ExecuteSql(sqlAddUser)) throw new Exception("Failed to add user.");

            return Ok();
        }

        throw new Exception("Failed to register user.");
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public IActionResult Login(UserForLoginDto userForLogin)
    {
        var sqlForHashAndSalt = @"SELECT 
                [PasswordHash],
                [PasswordSalt] FROM TutorialAppSchema.Auth WHERE Email = '" + userForLogin.Email + "'";

        var userForConfirmation = _dataContext.LoadDataSingle<UserForLoginConfirmationDto>(sqlForHashAndSalt);
        var passwordHash = _authHelper.GetPasswordHash(userForLogin.Password, userForConfirmation.PasswordSalt);

        if (!AuthHelper.ConstantTimeComparison(passwordHash, userForConfirmation.PasswordHash)) return StatusCode(401, "Incorrect password!");

        var userIdSql = "SELECT UserId FROM TutorialAppSchema.Users WHERE Email = '" + userForLogin.Email + "'";
        var userId = _dataContext.LoadDataSingle<int>(userIdSql);

        return Ok(new Dictionary<string, string>
        {
            { "token", _authHelper.CreateToken(userId) }
        });
    }

    [HttpGet("RefreshToken")]
    public string RefreshToken()
    {
        var userIdSql = @"
                SELECT UserId FROM TutorialAppSchema.Users WHERE UserId = '" + User.FindFirst("userId")?.Value + "'";

        var userId = _dataContext.LoadDataSingle<int>(userIdSql);

        return _authHelper.CreateToken(userId);
    }
}