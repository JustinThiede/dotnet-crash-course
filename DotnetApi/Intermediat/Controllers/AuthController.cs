using System.Data;
using System.Security.Cryptography;
using Dapper;
using Intermediate.Data;
using Intermediate.Dtos;
using Intermediate.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace Intermediate.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly DataContextDapper _dapper;
    private readonly AuthHelper _authHelper;

    public AuthController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
        _authHelper = new AuthHelper(config);
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public IActionResult Register(UserForRegistrationDto userForRegistration)
    {
        if (userForRegistration.Password != userForRegistration.PasswordConfirm) throw new Exception("Passwords do not match!");

        var sqlCheckUserExists = "SELECT Email FROM TutorialAppSchema.Auth WHERE Email = '" +
                                 userForRegistration.Email + "'";
        var existingUsers = _dapper.LoadData<string>(sqlCheckUserExists);

        if (existingUsers.Count() != 0) throw new Exception("User with this email already exists!");

        var userForSetPassword = new UserForLoginDto()
        {
            Email = userForRegistration.Email,
            Password = userForRegistration.Password
        };

        if (!_authHelper.SetPassword(userForSetPassword)) throw new Exception("Failed to register user.");

        var sqlAddUser = @"EXEC TutorialAppSchema.spUser_Upsert
                            @FirstName = '" + userForRegistration.FirstName +
                         "', @LastName = '" + userForRegistration.LastName +
                         "', @Email = '" + userForRegistration.Email +
                         "', @Gender = '" + userForRegistration.Gender +
                         "', @Active = 1" +
                         ", @JobTitle = '" + userForRegistration.JobTitle +
                         "', @Department = '" + userForRegistration.Department +
                         "', @Salary = '" + userForRegistration.Salary + "'";

        if (_dapper.ExecuteSql(sqlAddUser)) return Ok();
        throw new Exception("Failed to add user.");
    }

    [HttpPut("ResetPassword")]
    public IActionResult ResetPassword(UserForLoginDto userForSetPassword)
    {
        if (_authHelper.SetPassword(userForSetPassword)) return Ok();
        throw new Exception("Failed to update password!");
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public IActionResult Login(UserForLoginDto userForLogin)
    {
        const string sqlForHashAndSalt = @"EXEC TutorialAppSchema.spLoginConfirmation_Get @Email = @EmailParam";

        var sqlParameters = new DynamicParameters();
        sqlParameters.Add("@EmailParam", userForLogin.Email, DbType.String);

        var userForConfirmation = _dapper.LoadDataSingleWithParameters<UserForLoginConfirmationDto>(sqlForHashAndSalt, sqlParameters);
        var passwordHash = _authHelper.GetPasswordHash(userForLogin.Password, userForConfirmation.PasswordSalt);

        if (passwordHash.Where((t, index) => t != userForConfirmation.PasswordHash[index]).Any()) return StatusCode(401, "Incorrect password!");

        var userIdSql = @"SELECT UserId FROM TutorialAppSchema.Users WHERE Email = '" + userForLogin.Email + "'";
        var userId = _dapper.LoadDataSingle<int>(userIdSql);

        return Ok(new Dictionary<string, string>
        {
            { "token", _authHelper.CreateToken(userId) }
        });
    }

    [HttpGet("RefreshToken")]
    public string RefreshToken()
    {
        var userIdSql = @"SELECT UserId FROM TutorialAppSchema.Users WHERE UserId = '" + User.FindFirst("userId")?.Value + "'";
        var userId = _dapper.LoadDataSingle<int>(userIdSql);

        return _authHelper.CreateToken(userId);
    }
}