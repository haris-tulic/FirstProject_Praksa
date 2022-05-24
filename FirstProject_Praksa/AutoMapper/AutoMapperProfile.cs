using AutoMapper;
using FirstProject_Praksa.Database;
using FirstProject_Praksa.Dto;

namespace FirstProject_Praksa.AutoMapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AddCharacterDtos, Character>();
            CreateMap<GetCharacterDtos, Character>().ReverseMap();
            CreateMap<UpdateCharacterDtos, Character>().ReverseMap();
        }
    }
}
