using Microsoft.EntityFrameworkCore;

namespace FirstProject_Praksa.Database
{
    public class DataContext:DbContext
    {
        public DataContext()
        {
                
        }
        public DataContext(DbContextOptions<DataContext> options):base(options)
        {

        }
        public virtual DbSet<Character> Characters { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
