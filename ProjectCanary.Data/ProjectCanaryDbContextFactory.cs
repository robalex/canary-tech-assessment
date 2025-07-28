using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;

namespace ProjectCanary.Data
{
    internal class ProjectCanaryDbContextFactory : IDesignTimeDbContextFactory<ProjectCanaryDbContext>
    {
        public ProjectCanaryDbContext CreateDbContext(string[] args) 
        {
            var loggerFactory = LoggerFactory.Create(builder => {
                builder
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Information); // Or Debug, Trace
            });

            Console.WriteLine("Creating ProjectCanaryDbContext...");
            var optionsBuilder = new DbContextOptionsBuilder<ProjectCanaryDbContext>();
            optionsBuilder.UseLoggerFactory(loggerFactory)
                .EnableSensitiveDataLogging()
                .UseSnakeCaseNamingConvention()
                .UseNpgsql(args[0]);
            Console.WriteLine($"Creating DbContext with connection string: {args[0]}");
            //var connectionString = "Host=localhost;Port=5433;Database=project_canary_takehome;Username=project_canary_takehome;Password=giveemissionsthebird";
            var connectionString = args[0];
            optionsBuilder.UseNpgsql(connectionString);

            return new ProjectCanaryDbContext(optionsBuilder.Options);
        }
    }
}
