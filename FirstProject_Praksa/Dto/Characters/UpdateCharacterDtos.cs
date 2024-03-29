﻿using FirstProject_Praksa.Enum;

namespace FirstProject_Praksa.Dto.Characters
{
    public class UpdateCharacterDtos
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
