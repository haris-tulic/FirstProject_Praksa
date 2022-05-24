using FirstProject_Praksa.Enum;

namespace FirstProject_Praksa.Models
{
    public class CharacterModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Haris";
        public int LifePoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RPGClass Class { get; set; } = RPGClass.Knight;
    }
}
