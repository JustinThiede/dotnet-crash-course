using AutoMapper;
using Basic.Data;
using Basic.Dtos;
using Basic.Models;
using Microsoft.AspNetCore.Mvc;

namespace Basic.Controllers;

[ApiController]
[Route("[controller]")]
public class UserSalaryControllerEF : ControllerBase
{
    private DataContextEF _entityFramework;

    private IMapper _mapper;

    public UserSalaryControllerEF(IConfiguration config)
    {
        _entityFramework = new DataContextEF(config);
        _mapper = new Mapper(new MapperConfiguration(cfg => { cfg.CreateMap<UserSalaryToAddDto, UserSalary>(); }));
    }

    [HttpGet("GetUsersSalary")]
    public IEnumerable<UserSalary> GetUsersSalary()
    {
        var usersSalary = _entityFramework.UserSalary.ToList<UserSalary>();

        return usersSalary;
    }

    [HttpGet("GetSingleUserSalary/{userId}")]
    public UserSalary GetSingleUserSalary(int userId)
    {
        var usersSalary = _entityFramework.UserSalary.FirstOrDefault(u => u.UserId == userId);

        if (usersSalary != null) return usersSalary;

        throw new Exception("Failed to Get User Salary");
    }

    [HttpPut]
    public IActionResult EditUserSalary(UserSalary userSalary)
    {
        var userSalaryDb = _entityFramework.UserSalary.FirstOrDefault(u => u.UserId == userSalary.UserId);

        if (userSalaryDb == null) throw new Exception("Failed to Get User Salary");

        userSalaryDb.Salary = userSalary.Salary;

        if (_entityFramework.SaveChanges() > 0) return Ok();

        throw new Exception("Failed to Update User Salary");
    }

    [HttpPost]
    public IActionResult AddUserSalary(UserSalaryToAddDto userSalaryToAdd)
    {
        var userSalaryDb = _mapper.Map<UserSalary>(userSalaryToAdd);

        var user = _entityFramework.Users.SingleOrDefault(user => user.UserId == userSalaryDb.UserId);

        if (user == null) throw new Exception("User does not exist");

        _entityFramework.Add(userSalaryDb);

        if (_entityFramework.SaveChanges() > 0) return Ok();

        throw new Exception("Failed to Add User Salary");
    }

    [HttpDelete("DeleteUserSalary/{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
        var userSalaryDb = _entityFramework.UserSalary.FirstOrDefault(u => u.UserId == userId);

        if (userSalaryDb == null) throw new Exception("Failed to Get User Salary");

        _entityFramework.Remove(userSalaryDb);

        if (_entityFramework.SaveChanges() > 0) return Ok();

        throw new Exception("Failed to Delete User Salary");
    }
}