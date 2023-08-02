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
    private DataContextEF _entityFramework;

    private IMapper _mapper;

    public UserJobInfoControllerEF(IConfiguration config)
    {
        _entityFramework = new DataContextEF(config);
        _mapper = new Mapper(new MapperConfiguration(cfg => { cfg.CreateMap<UserJobInfoToAddDto, UserJobInfo>(); }));
    }

    [HttpGet("GetUsersJobInfo")]
    public IEnumerable<UserJobInfo> GetUsersJobInfo()
    {
        var usersJobInfo = _entityFramework.UserJobInfo.ToList<UserJobInfo>();

        return usersJobInfo;
    }

    [HttpGet("GetSingleUserJobInfo/{userId}")]
    public UserJobInfo GetSingleUserJobInfo(int userId)
    {
        var usersJobInfo = _entityFramework.UserJobInfo.FirstOrDefault(u => u.UserId == userId);

        if (usersJobInfo != null) return usersJobInfo;

        throw new Exception("Failed to Get User Job Info");
    }

    [HttpPut]
    public IActionResult EditUserJobInfo(UserJobInfo userJobInfo)
    {
        var userJobInfoDb = _entityFramework.UserJobInfo.FirstOrDefault(u => u.UserId == userJobInfo.UserId);

        if (userJobInfoDb == null) throw new Exception("Failed to Get User Job Info");

        userJobInfoDb.JobTitle = userJobInfo.JobTitle;
        userJobInfoDb.Department = userJobInfo.Department;

        if (_entityFramework.SaveChanges() > 0) return Ok();

        throw new Exception("Failed to Update User Job Info");
    }

    [HttpPost]
    public IActionResult AddUserJobInfo(UserJobInfoToAddDto userJobInfoToAdd)
    {
        var userJobInfoDb = _mapper.Map<UserJobInfo>(userJobInfoToAdd);
        var user = _entityFramework.Users.SingleOrDefault(user => user.UserId == userJobInfoDb.UserId);

        if (user == null) throw new Exception("User does not exist");

        _entityFramework.Add(userJobInfoDb);

        if (_entityFramework.SaveChanges() > 0) return Ok();

        throw new Exception("Failed to Add User Job Info");
    }

    [HttpDelete("DeleteUserJobInfo/{userId}")]
    public IActionResult DeleteUserJobInfo(int userId)
    {
        var userJobInfoDb = _entityFramework.UserJobInfo.FirstOrDefault(u => u.UserId == userId);

        if (userJobInfoDb == null) throw new Exception("Failed to Get User Job Info");

        _entityFramework.Remove(userJobInfoDb);

        if (_entityFramework.SaveChanges() > 0) return Ok();

        throw new Exception("Failed to Delete User Job Info");
    }
}