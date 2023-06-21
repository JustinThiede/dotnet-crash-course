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
    }
}