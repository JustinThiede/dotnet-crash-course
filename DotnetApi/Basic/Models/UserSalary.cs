namespace Basic.Models;
public partial class UserSalary
{
    public int UserId { get; set; }
    public decimal Salary { get; set; }

    public User User { get; set; }
}