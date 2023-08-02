using AutoMapper;
using Intermediate.Data;
using Intermediate.Dtos;
using Intermediate.Models;
using Microsoft.AspNetCore.Mvc;

namespace Intermediate.Controllers;

[ApiController]
[Route("[controller]")]
public class UserControllerEF : ControllerBase
{
    private DataContextEF _entityFramework;

    private IMapper _mapper;

    private IUserRepository _userRepository;

    public UserControllerEF(IConfiguration config, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _entityFramework = new DataContextEF(config);
        _mapper = new Mapper(new MapperConfiguration(cfg => { cfg.CreateMap<UserToAddDto, User>(); }));
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
        var userDb = _mapper.Map<User>(userToAdd);

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