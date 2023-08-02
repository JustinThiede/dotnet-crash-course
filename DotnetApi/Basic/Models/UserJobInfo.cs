﻿namespace Intermediate.Models;
public partial class UserJobInfo
{
    public int UserId { get; set; }
    public string JobTitle { get; set; }
    public string Department { get; set; }

    public User User { get; set; }

    public UserJobInfo()
    {
        if (JobTitle == null) JobTitle = "";

        if (Department == null) Department = "";
    }
}