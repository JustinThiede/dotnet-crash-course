using AutoMapper;
using Intermediate.Data;
using Intermediate.Dtos;
using Intermediate.Models;
using Microsoft.AspNetCore.Mvc;

namespace Intermediate.Controllers;

[ApiController]
[Route("[controller]")]
public class UserJobInfoControllerEF : ControllerBase
{
    private IMapper _mapper;

    private IUserRepository _userRepository;

    public UserJobInfoControllerEF(IUserRepository userRepository)
    {
        _mapper = new Mapper(new MapperConfiguration(cfg => { cfg.CreateMap<UserJobInfoToAddDto, UserJobInfo>(); }));
        _userRepository = userRepository;
    }

    [HttpGet("GetUsersJobInfo")]
    public IEnumerable<UserJobInfo> GetUsersJobInfo()
    {
        var usersJobInfo = _userRepository.GetUsersJobInfo();

        return usersJobInfo;
    }

    [HttpGet("GetSingleUserJobInfo/{userId}")]
    public UserJobInfo GetSingleUserJobInfo(int userId)
    {
        return _userRepository.GetSingleUserJobInfo(userId);
    }

    [HttpPut]
    public IActionResult EditUserJobInfo(UserJobInfo userJobInfo)
    {
        var userJobInfoDb = _userRepository.GetSingleUserJobInfo(userJobInfo.UserId);

        userJobInfoDb.JobTitle = userJobInfo.JobTitle;
        userJobInfoDb.Department = userJobInfo.Department;

        if (_userRepository.SaveChanges()) return Ok();

        throw new Exception("Failed to Update User Job Info");
    }

    [HttpPost]
    public IActionResult AddUserJobInfo(UserJobInfoToAddDto userJobInfoToAdd)
    {
        var userJobInfoDb = _mapper.Map<UserJobInfo>(userJobInfoToAdd);

        try
        {
            _userRepository.GetSingleUser(userJobInfoToAdd.UserId);
        }
        catch (Exception e)
        {
            throw new Exception("User does not exist");
        }

        _userRepository.AddEntity<UserJobInfo>(userJobInfoDb);

        if (_userRepository.SaveChanges()) return Ok();

        throw new Exception("Failed to Add User Job Info");
    }

    [HttpDelete("DeleteUserJobInfo/{userId}")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
        var userJobInfoDb = _userRepository.GetSingleUserJobInfo(userId);

        _userRepository.RemoveEntity<UserJobInfo>(userJobInfoDb);

        if (_userRepository.SaveChanges()) return Ok();

        throw new Exception("Failed to Delete User Job Info");
    }
}