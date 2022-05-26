using FirstProject_Praksa.Database;
using FirstProject_Praksa.Dto.Characters;

namespace FirstProject_Praksa.Service.CharacterService
{
    public interface ICharacterService
    {
        public Task<ServiceResponse<List<GetCharacterDtos>>> GetAll();
        public Task<ServiceResponse<GetCharacterDtos>> Get(int id);
        public Task<ServiceResponse<List<GetCharacterDtos>>> AddNewCharacter(AddCharacterDtos character);
        public Task<ServiceResponse<List<GetCharacterDtos>>> DeleteCharacter(int id);
        public Task<ServiceResponse<GetCharacterDtos>> Update(UpdateCharacterDtos updateCharacter);
        public Task<ServiceResponse<GetCharacterDtos>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill);
    }
}
