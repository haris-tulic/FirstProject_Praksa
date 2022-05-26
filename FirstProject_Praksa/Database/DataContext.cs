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
        public virtual DbSet<Weapon> Weapons { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }    
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Skill>().HasData(
                new Skill { Id=1,Name="Fireball",Damage=30},
                new Skill { Id=2,Name="Frenzi",Damage=20},
                new Skill { Id = 3, Name = "Blizzard", Damage = 50 }
            );
        }
    }
}
