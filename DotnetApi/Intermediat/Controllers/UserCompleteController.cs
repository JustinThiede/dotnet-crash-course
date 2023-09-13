using Intermediate.Models;
using Microsoft.AspNetCore.Mvc;
using Intermediate.Data;

namespace Intermediate.Controllers;

[ApiController]
[Route("[controller]")]
public class UserCompleteController : ControllerBase
{
    private readonly DataContextDapper _dapper;

    public UserCompleteController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet("GetUsers/{userId}/{isActive}")]
    // public IEnumerable<User> GetUsers()
    public IEnumerable<UserComplete> GetUsers(int userId, bool isActive)
    {
        var sql = @"EXEC TutorialAppSchema.spUsers_Get";
        var parameters = "";

        if (userId != 0) parameters += ", @UserId=" + userId.ToString();
        if (isActive) parameters += ", @Active=" + isActive.ToString();

        if (parameters.Length > 0) sql += parameters[1..];

        var users = _dapper.LoadData<UserComplete>(sql);
        return users;
    }

    [HttpPut("UpsertUser")]
    public IActionResult UpsertUser(UserComplete user)
    {
        var sql = @"EXEC TutorialAppSchema.spUser_Upsert
            @FirstName = '" + user.FirstName +
                  "', @LastName = '" + user.LastName +
                  "', @Email = '" + user.Email +
                  "', @Gender = '" + user.Gender +
                  "', @Active = '" + user.Active +
                  "', @JobTitle = '" + user.JobTitle +
                  "', @Department = '" + user.Department +
                  "', @Salary = '" + user.Salary +
                  "', @UserId = " + user.UserId;

        if (_dapper.ExecuteSql(sql)) return Ok();

        throw new Exception("Failed to Update User");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        var sql = @"TutorialAppSchema.spUser_Delete
            @UserId = " + userId.ToString();

        if (_dapper.ExecuteSql(sql)) return Ok();

        throw new Exception("Failed to Delete User");
    }
}