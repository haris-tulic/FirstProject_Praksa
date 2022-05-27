using FirstProject_Praksa.Database;
using FirstProject_Praksa.Dto.Fight;

namespace FirstProject_Praksa.Service.Fight
{
    public interface IFIghtService
    {
        Task<ServiceResponse<AttackResultDto>> FightSkill(SkillAttackDto fight);

        Task<ServiceResponse<AttackResultDto>> FightWeapon(WeaponAttackDto fight);
        Task<ServiceResponse<FightResultDto>> FinalFight(FightRequestDto fight);
        Task<ServiceResponse<List<HighScoreDto>>> GetHighScore();

    }
}
