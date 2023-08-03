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
    private IMapper _mapper;

    private IUserRepository _userRepository;

    public UserControllerEF(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _mapper = new Mapper(new MapperConfiguration(cfg => { cfg.CreateMap<UserToAddDto, User>(); }));
    }

    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        var users = _userRepository.GetUsers();

        return users;
    }

    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {
        return _userRepository.GetSingleUser(userId);
    }

    [HttpPut]
    public IActionResult EditUser(User user)
    {
        var userDb = _userRepository.GetSingleUser(user.UserId);

        userDb.Active = user.Active;
        userDb.FirstName = user.FirstName;
        userDb.LastName = user.LastName;
        userDb.Email = user.Email;
        userDb.Gender = user.Gender;

        if (_userRepository.SaveChanges()) return Ok();

        throw new Exception("Failed to Update User");
    }

    [HttpPost]
    public IActionResult AddUser(UserToAddDto userToAdd)
    {
        var userDb = _mapper.Map<User>(userToAdd);

        _userRepository.AddEntity<User>(userDb);

        if (_userRepository.SaveChanges()) return Ok();

        throw new Exception("Failed to Add User");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        var userDb = _userRepository.GetSingleUser(userId);

        if (userDb == null) throw new Exception("Failed to Get User");

        _userRepository.RemoveEntity<User>(userDb);

        if (_userRepository.SaveChanges()) return Ok();

        throw new Exception("Failed to Delete User");
    }
}