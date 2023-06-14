using Basic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Basic.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public UserController()
        {
        }

        [HttpGet("GetUsers/{testValue}")]
        public string[] GetUsers(string testValue)
        {
            return new string[] { "user1", "user2", testValue};
        }
    }
}