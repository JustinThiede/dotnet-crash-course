namespace Basic.Models;
public partial class UserJobInfo
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public bool Active { get; set; }

    public UserJobInfo()
    {
        if (FirstName == null)
        {
            FirstName = "";
        }

        if (LastName == null)
        {
            LastName = "";
        }

        if (Email == null)
        {
            Email = "";
        }

        if (Gender == null)
        {
            Gender = "";
        }
    }
}