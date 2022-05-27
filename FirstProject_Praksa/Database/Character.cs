using FirstProject_Praksa.Enum;

namespace FirstProject_Praksa.Database
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Haris";
        public int LifePoints { get; set; } = 100;
        public int Strength { get; set; } =10 ;
        public int Defense { get; set; } =10 ;
        public int Intelligence { get; set; } = 10;
        public RPGClass Class { get; set; } = RPGClass.Knight;
        public User User {get; set; }
        public Weapon Weapon { get; set; }
        public List<Skill> Skills { get; set; }
        public int Fights { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }
    }
}
