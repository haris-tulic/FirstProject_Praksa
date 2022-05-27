using AutoMapper;
using FirstProject_Praksa.Database;
using FirstProject_Praksa.Dto.Fight;
using Microsoft.EntityFrameworkCore;

namespace FirstProject_Praksa.Service.Fight
{
    public class FightService : IFIghtService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public FightService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<AttackResultDto>> FightWeapon(WeaponAttackDto fight)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters.Include(a => a.Skills).Include(a => a.Weapon).FirstOrDefaultAsync(a => a.Id == fight.AttackerId);
                var opponent = await _context.Characters.Include(a => a.Skills).Include(a => a.Weapon).FirstOrDefaultAsync(a => a.Id == fight.OpponentId);
                if (attacker == null || opponent == null)
                {
                    response.Succees = false;
                    response.Message = "Couldn't find attacker or opponent! Try again.";
                }
                int damage = DoWeaponAttack(attacker, opponent);

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

        private static int DoWeaponAttack(Character? attacker, Character? opponent)
        {
            var damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
            damage -= opponent.Defense;
            if (damage > 0)
            {
                opponent.LifePoints -= damage;
            }

            return damage;
        }

        public async Task<ServiceResponse<AttackResultDto>> FightSkill(SkillAttackDto fight)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters.Include(a => a.Skills).Include(a => a.Weapon).FirstOrDefaultAsync(a => a.Id == fight.AttackerId);
                var opponent = await _context.Characters.Include(a => a.Skills).Include(a => a.Weapon).FirstOrDefaultAsync(a => a.Id == fight.OpponentId);
                if (attacker == null || opponent == null)
                {
                    response.Succees = false;
                    response.Message = "Can not find attacker or opponent!";
                    return response;
                }
                var skill = attacker.Skills.FirstOrDefault(s => s.Id == fight.SkillId);
                int damage = DoSkillAttack(attacker, opponent, skill);
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

        private static int DoSkillAttack(Character? attacker, Character? opponent, Skill? skill)
        {
            var damage = skill.Damage + (new Random().Next(attacker.Intelligence));

            damage = -opponent.Defense;
            if (damage > 0)
            {
                opponent.LifePoints = -damage;
            }

            return damage;
        }

        public async Task<ServiceResponse<FightResultDto>> FinalFight(FightRequestDto fight)
        {
            var response = new ServiceResponse<FightResultDto>();
            try
            {
                var characters = await _context.Characters
                    .Include(c => c.Weapon).Include(c => c.Skills)
                    .Where(c => fight.CharactersIds.Contains(c.Id)).ToListAsync();
                var defeated = false;
                while (!defeated)
                {
                    foreach (var attacker in characters)
                    {
                        var opponents = characters.Where(a => a.Id != attacker.Id).ToList();
                        var opponent=opponents[new Random().Next(opponents.Count)];
                        var damage = 0;
                        var attackUser = string.Empty;
                        var useWeapon = new Random().Next(2)==0;
                        if (useWeapon)
                        {
                            attackUser = attacker.Weapon.Name;
                            damage = DoWeaponAttack(attacker, opponent);
                        }
                        else
                        {
                            var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
                            attackUser = skill.Name;
                            damage = DoSkillAttack(attacker, opponent, skill);
                        }
                        response.Data.Log.Add($"{attacker.Name} attacked {opponent.Name} with {attackUser} with {(damage >= 0 ? damage : 0)} damage");
                        if (opponent.LifePoints<=0)
                        {
                            defeated = true;
                            attacker.Victories++;
                            opponent.Defeats++;
                            response.Data.Log.Add($"{opponent.Name} is defeated!");
                            response.Data.Log.Add($"{attacker.Name} wins with {attacker.LifePoints} left!");
                            break;
                        }
                    }
                }
                characters.ForEach(c =>
                {
                    c.Fights++;
                    c.LifePoints = 100;
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Succees = false;
            }
            return response;
        }

        public async Task<ServiceResponse<List<HighScoreDto>>> GetHighScore()
        {
            var response = new ServiceResponse<List<HighScoreDto>>();
            var highScore = await _context.Characters.Where(c => c.Fights > 0).OrderByDescending(c => c.Victories).ThenBy(c => c.Defeats).ToListAsync();
            response.Data = _mapper.Map<List<HighScoreDto>>(highScore);
            return response;
        }
    }
}
