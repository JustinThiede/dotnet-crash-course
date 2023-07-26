using Basic.Data;
using Basic.Dtos;
using Basic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Basic.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserControllerEF : ControllerBase
    {
        private DataContextEF _entityFramework;

        public UserControllerEF(IConfiguration config)
        {
            _entityFramework = new DataContextEF(config);
        }

        [HttpGet("GetUsers")]
        public IEnumerable<User> GetUsers()
        {
            var users = _entityFramework.Users.ToList<User>();

            return users;
        }

        [HttpGet("GetSingleUser/{userId}")]
        public User GetSingleUser(int userId)
        {
            var user = _entityFramework.Users.FirstOrDefault(u => u.UserId == userId);

            if (user != null) return user;

            throw new Exception("Failed to Get User");
        }

        [HttpPut]
        public IActionResult EditUser(User user)
        {
            var userDb = _entityFramework.Users.FirstOrDefault(u => u.UserId == user.UserId);

            if (userDb == null) throw new Exception("Failed to Get User");

            userDb.Active = user.Active;
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;

            if (_entityFramework.SaveChanges() > 0) return Ok();

            throw new Exception("Failed to Update User");
        }

        [HttpPost]
        public IActionResult AddUser(UserToAddDto userToAdd)
        {
            var userDb = new User();

            userDb.Active = userToAdd.Active;
            userDb.FirstName = userToAdd.FirstName;
            userDb.LastName = userToAdd.LastName;
            userDb.Email = userToAdd.Email;
            userDb.Gender = userToAdd.Gender;

            _entityFramework.Add(userDb);
            if (_entityFramework.SaveChanges() > 0) return Ok();

            throw new Exception("Failed to Add User");
        }

        [HttpDelete("DeleteUser/{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            var userDb = _entityFramework.Users.FirstOrDefault(u => u.UserId == userId);

            if (userDb == null) throw new Exception("Failed to Get User");

            _entityFramework.Remove(userDb);

            if (_entityFramework.SaveChanges() > 0) return Ok();

            throw new Exception("Failed to Delete User");
        }
    }
}