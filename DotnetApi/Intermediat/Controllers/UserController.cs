using Intermediate.Data;
using Intermediate.Dtos;
using Intermediate.Models;
using Microsoft.AspNetCore.Mvc;

namespace Intermediate.Controllers
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
        public IActionResult AddUser(UserToAddDto userToAdd)
        {
            var sql = @$"
            INSERT INTO TutorialAppSchema.Users(
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active]
            ) VALUES (
                '{userToAdd.FirstName}',
                '{userToAdd.LastName}',
                '{userToAdd.Email}',
            '{userToAdd.Gender}',
            '{userToAdd.Active}')";

            if (_dapper.ExecuteSql(sql)) return Ok();

            throw new Exception("Failed to Add User");
        }

        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            var sql = @"
            DELETE FROM TutorialAppSchema.Users 
                WHERE UserId = " + userId.ToString();

            Console.WriteLine(sql);

            if (_dapper.ExecuteSql(sql)) return Ok();

            throw new Exception("Failed to Delete User");
        }
    }
}