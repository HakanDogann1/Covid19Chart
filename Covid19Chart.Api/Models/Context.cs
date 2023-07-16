using Microsoft.EntityFrameworkCore;

namespace Covid19Chart.Api.Models
{
    public class Context:DbContext
    {
        public Context(DbContextOptions<Context> options):base(options)
        {
            
        }
        public DbSet<Covid> Covids { get; set; }
    }
}
