using Intermediate.Models;

namespace Intermediate.Data;

public class UserRepository : IUserRepository
{
    private DataContextEF _entityFramework;

    public UserRepository(IConfiguration config)
    {
        _entityFramework = new DataContextEF(config);
    }

    public IEnumerable<User> GetUsers()
    {
        var users = _entityFramework.Users.ToList<User>();

        return users;
    }

    public IEnumerable<UserJobInfo> GetUsersJobInfo()
    {
        var usersJobInfo = _entityFramework.UserJobInfo.ToList<UserJobInfo>();

        return usersJobInfo;
    }

    public IEnumerable<UserSalary> GetUsersSalary()
    {
        var usersSalary = _entityFramework.UserSalary.ToList<UserSalary>();

        return usersSalary;
    }

    public User GetSingleUser(int userId)
    {
        var user = _entityFramework.Users.FirstOrDefault(u => u.UserId == userId);

        if (user != null) return user;

        throw new Exception("Failed to Get User");
    }

    public UserJobInfo GetSingleUserJobInfo(int userId)
    {
        var usersJobInfo = _entityFramework.UserJobInfo.FirstOrDefault(u => u.UserId == userId);

        if (usersJobInfo != null) return usersJobInfo;

        throw new Exception("Failed to Get User Job Info");
    }

    public UserSalary GetSingleUserSalary(int userId)
    {
        var usersSalary = _entityFramework.UserSalary.FirstOrDefault(u => u.UserId == userId);

        if (usersSalary != null) return usersSalary;

        throw new Exception("Failed to Get User Salary");
    }

    public bool SaveChanges()
    {
        return _entityFramework.SaveChanges() > 0;
    }

    public void AddEntity<T>(T entityToAdd)
    {
        if (entityToAdd != null) _entityFramework.Add(entityToAdd);
    }

    public void RemoveEntity<T>(T entityToAdd)
    {
        if (entityToAdd != null) _entityFramework.Remove(entityToAdd);
    }
}