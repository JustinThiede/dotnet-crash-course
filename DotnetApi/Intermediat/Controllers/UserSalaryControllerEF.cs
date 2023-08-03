using AutoMapper;
using Intermediate.Data;
using Intermediate.Dtos;
using Intermediate.Models;
using Microsoft.AspNetCore.Mvc;

namespace Intermediate.Controllers;

[ApiController]
[Route("[controller]")]
public class UserSalaryControllerEF : ControllerBase
{
    private IMapper _mapper;

    private IUserRepository _userRepository;

    public UserSalaryControllerEF(IUserRepository userRepository)
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => { cfg.CreateMap<UserSalaryToAddDto, UserSalary>(); }));
        _userRepository = userRepository;
    }

    [HttpGet("GetUsersSalary")]
    public IEnumerable<UserSalary> GetUsersSalary()
    {
        var usersSalary = _userRepository.GetUsersSalary();

        return usersSalary;
    }

    [HttpGet("GetSingleUserSalary/{userId}")]
    public UserSalary GetSingleUserSalary(int userId)
    {
        return _userRepository.GetSingleUserSalary(userId);
    }

    [HttpPut]
    public IActionResult EditUserSalary(UserSalary userSalary)
    {
        var userSalaryDb = _userRepository.GetSingleUserSalary(userSalary.UserId);

        userSalaryDb.Salary = userSalary.Salary;

        if (_userRepository.SaveChanges()) return Ok();

        throw new Exception("Failed to Update User Salary");
    }

    [HttpPost]
    public IActionResult AddUserSalary(UserSalaryToAddDto userSalaryToAdd)
    {
        var userSalaryDb = _mapper.Map<UserSalary>(userSalaryToAdd);

        try
        {
            _userRepository.GetSingleUser(userSalaryToAdd.UserId);
        }
        catch (Exception e)
        {
            throw new Exception("User does not exist");
        }

        _userRepository.AddEntity<UserSalary>(userSalaryDb);

        if (_userRepository.SaveChanges()) return Ok();

        throw new Exception("Failed to Add User Salary");
    }

    [HttpDelete("DeleteUserSalary/{userId}")]
    public IActionResult DeleteUserSalary(int userId)
    {
        var userSalaryDb = _userRepository.GetSingleUserSalary(userId);

        _userRepository.RemoveEntity<UserSalary>(userSalaryDb);

        if (_userRepository.SaveChanges()) return Ok();

        throw new Exception("Failed to Delete User Salary");
    }
}