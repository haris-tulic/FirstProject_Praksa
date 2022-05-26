using AutoMapper;
using FirstProject_Praksa.Database;
using FirstProject_Praksa.Dto.Characters;
using FirstProject_Praksa.Dto.Skill;
using FirstProject_Praksa.Dto.Weapon;

namespace FirstProject_Praksa.AutoMapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddCharacterDtos, Character>();
            CreateMap<GetCharacterDtos, Character>().ReverseMap();
            CreateMap<UpdateCharacterDtos, Character>().ReverseMap();
            CreateMap<AddWeaponDto, Weapon>().ReverseMap();
            CreateMap<Weapon, GetWeaponDto>().ReverseMap();
            CreateMap<Skill, GetSkillDto>().ReverseMap();
        }
    }
}
