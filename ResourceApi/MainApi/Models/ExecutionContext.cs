using Microsoft.EntityFrameworkCore;

namespace MainApi.Models
{
    public class ExecutionContext : DbContext
    { 
        public ExecutionContext() : base() { }

        public DbSet<ExecutionTime> Profiling { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(@"host=35.233.102.116;database=postgres;user id=postgres;password=Septr@k@n800;");
            optionsBuilder.EnableSensitiveDataLogging(true);
        }
    }
}
