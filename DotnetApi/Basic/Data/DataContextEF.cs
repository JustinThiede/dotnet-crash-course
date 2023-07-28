using Basic.Models;
using Microsoft.EntityFrameworkCore;

namespace Basic.Data;

public class DataContextEF : DbContext
{
    private readonly IConfiguration _config;

    public DataContextEF(IConfiguration config)
    {
        _config = config;
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserSalary> UserSalary { get; set; }
    public virtual DbSet<UserJobInfo> UserJobInfo { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"),
                optionsBuilder => optionsBuilder.EnableRetryOnFailure());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("TutorialAppSchema");

        modelBuilder.Entity<User>().HasKey(u => u.UserId);

        modelBuilder.Entity<User>().HasOne(u => u.UserSalary) // User has one UserSalary
            .WithOne(s => s.User) // UserSalary has one User
            .HasForeignKey<UserSalary>(s => s.UserId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserSalary>().HasKey(u => u.UserId);

        modelBuilder.Entity<User>().HasOne(u => u.UserJobInfo) // User has one UserJobInfo
            .WithOne(j => j.User) // UserJobInfo has one User
            .HasForeignKey<UserJobInfo>(j => j.UserId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserJobInfo>().HasKey(u => u.UserId);
    }
}