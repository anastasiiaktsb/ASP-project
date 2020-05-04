using Microsoft.EntityFrameworkCore;

namespace WorkersApp.Models
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> User { set; get; }
        public DbSet<Worker> Worker { get; set; }
    }
}
