using FirstProject_Praksa.Database;
using FirstProject_Praksa.Dto.Fight;
using Microsoft.EntityFrameworkCore;

namespace FirstProject_Praksa.Service.Fight
{
    public class FightService : IFIghtService
    {
        private readonly DataContext _context;

        public FightService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<AttackResultDto>> Fight(WeaponAttackDto fight)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters.Include(a => a.Skills).Include(a => a.Weapon).FirstOrDefaultAsync(a => a.Id == fight.AttackerId);
                var opponent=await _context.Characters.Include(a=>a.Skills).Include(a=>a.Weapon).FirstOrDefaultAsync(a => a.Id == fight.OpponentId);
                if (attacker==null || opponent==null)
                {
                    response.Succees = false;
                    response.Message = "Couldn't find attacker or opponent! Try again.";
                }
                var damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
                damage = -opponent.Defense;
                if (damage>0)
                {
                    opponent.LifePoints = -damage;
                }
                if (opponent.LifePoints<=0)
                {
                    response.Message = $"{opponent.Name} has been defeated!";
                }
                await _context.SaveChangesAsync();
                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    AttackerHP = attacker.LifePoints,
                    Damage = damage,
                    Opponent = opponent.Name,
                    OpponentHP = opponent.LifePoints,
                };
                response.Succees = true;
            }
            catch (Exception ex)
            {
                response.Succees = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<AttackResultDto>> FightSkill(SkillAttackDto fight)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker=await _context.Characters.Include(a=>a.Skills).Include(a=>a.Weapon).FirstOrDefaultAsync(a=> a.Id==fight.AttackerId);
                var opponent= await _context.Characters.Include(a => a.Skills).Include(a => a.Weapon).FirstOrDefaultAsync(a => a.Id == fight.OpponentId);
                if (attacker==null || opponent==null)
                {
                    response.Succees = false;
                    response.Message = "Can not find attacker or opponent!";
                    return response;
                }
                var skill = attacker.Skills.FirstOrDefault(s => s.Id == fight.SkillId);
                var damage = skill.Damage + (new Random().Next(attacker.Intelligence));

                damage = -opponent.Defense;
                if (damage > 0)
                {
                    opponent.LifePoints = -damage;
                }
                if (opponent.LifePoints <= 0)
                {
                    response.Message = $"{opponent.Name} has been defeated!";
                }
                await _context.SaveChangesAsync();
                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    AttackerHP = attacker.LifePoints,
                    Damage = damage,
                    Opponent = opponent.Name,
                    OpponentHP = opponent.LifePoints,
                };
                response.Succees = true;
            }
            catch (Exception ex)
            {
                response.Succees = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
