using Microsoft.EntityFrameworkCore;
using MyProject.Entities;

namespace MyProject.Database;

public class DatabaseContext:DbContext
{
    public DbSet<User> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder
            .UseMySQL(Environment.GetEnvironmentVariable("MYSQL_CONNECTION") ??
                      "server=localhost;database=MyProject;user=root;password=12345678")
            .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
            .EnableSensitiveDataLogging();
}