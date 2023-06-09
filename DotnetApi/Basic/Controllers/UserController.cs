using Basic.Data;
using Basic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Basic.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private DataContextDapper _dapper;

        public UserController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet("TestConnection")]
        public DateTime TestConnection()
        {
            return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
        }

        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            var sql = @"
                SELECT [UserId]
                      ,[FirstName]
                      ,[LastName]
                      ,[Email]
                      ,[Gender]
                      ,[Active]
                  FROM [DotNetCourseDatabase].[TutorialAppSchema].[Users]";
            var users = _dapper.LoadData<User>(sql);

            return users;
        }

        [HttpGet("GetSingleUser/{userId}")]
        public User GetSingleUser(int userId)
        {
            var sql = @"
                SELECT [UserId]
                      ,[FirstName]
                      ,[LastName]
                      ,[Email]
                      ,[Gender]
                      ,[Active]
                  FROM [DotNetCourseDatabase].[TutorialAppSchema].[Users]
                    WHERE UserId = " + userId.ToString();
            var user = _dapper.LoadDataSingle<User>(sql);

            return user;
        }

        [HttpPut]
        public IActionResult EditUser(User user)
        {
            var sql = @$"
            UPDATE TutorialAppSchema.Users
                SET [FirstName] = '{user.FirstName}',
                    [LastName] = '{user.LastName}',
                    [Email] = '{user.Email}',
                    [Gender] = '{user.Gender}',
                    [Active] = '{user.Active}' 
                WHERE UserId = '{user.UserId}'";

            if (_dapper.ExecuteSql(sql)) return Ok();

            throw new Exception("Failed to Update User");
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            var sql = @$"
            INSERT INTO TutorialAppSchema.Users(
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active]
            ) VALUES (
                '{user.FirstName}',
                '{user.LastName}',
                '{user.Email}',
            '{user.Gender}',
            '{user.Active}')";

            if (_dapper.ExecuteSql(sql)) return Ok();

            throw new Exception("Failed to Add User");
        }
    }
}