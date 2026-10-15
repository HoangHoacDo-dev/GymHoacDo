using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<Member> Members { get; set; }
    public DbSet<Package> Packages { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<FreezeLog> FreezeLogs { get; set; }
    public DbSet<CheckInLog> CheckInLogs { get; set; }
    public DbSet<Employee> Employees { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

       modelBuilder.Entity<Package>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Package>().HasData(
            new Package { Id = 1, Name = "Gym 1 Tháng", DurationInDays = 30, Price = 500000 },
            new Package { Id = 2, Name = "Yoga 1 Năm", DurationInDays = 365, Price = 4500000 }
        );
        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, FullName = "Quản lý (Admin)", AccountName = "admin", Password = BCrypt.Net.BCrypt.HashPassword("123"), Role = "Admin" },
            new Employee { Id = 2, FullName = "Lễ tân (Receptionist)", AccountName = "letan", Password = BCrypt.Net.BCrypt.HashPassword("123"), Role = "Receptionist" }
        );
    }
}