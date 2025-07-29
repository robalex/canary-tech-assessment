using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProjectCanary.Data
{
    internal class ProjectCanaryDbContextFactory : IDesignTimeDbContextFactory<ProjectCanaryDbContext>
    {
        public ProjectCanaryDbContext CreateDbContext(string[] args) 
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProjectCanaryDbContext>();

            var connectionString = args[0];
            optionsBuilder.UseNpgsql(connectionString);

            return new ProjectCanaryDbContext(optionsBuilder.Options);
        }
    }
}
