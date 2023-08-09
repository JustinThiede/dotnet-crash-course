using System.Security.Cryptography;
using Intermediate.Data;
using Intermediate.Dtos;
using Intermediate.Models;
using Microsoft.AspNetCore.Mvc;

namespace Intermediate.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dataContext;

        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _dataContext = new DataContextDapper(configuration);
            _configuration = configuration;
        }

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

            var passwordSaltPlusString = _configuration.GetSection("AppSettings:PasswordKey").Value + Convert.ToBase64String(passwordSalt);

            return Ok();
        }

        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLogin)
        {
            return Ok();
        }
    }
}
