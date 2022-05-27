using FirstProject_Praksa.Database;
using FirstProject_Praksa.Dto.Fight;
using FirstProject_Praksa.Service.Fight;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject_Praksa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FightController : ControllerBase
    {
        private readonly IFIghtService _service;

        public FightController(IFIghtService service)
        {
            _service = service;
        }
        [HttpPost("FightSkill")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> FightSkill(SkillAttackDto skill)
        {
            var result = await _service.FightSkill(skill);
            if (result == null)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("FightWeapon")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> FightWeapon(WeaponAttackDto weapon)
        {
            var result = await _service.FightWeapon(weapon);
            if (result == null)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPost("FinalFight")]
        public async Task<ActionResult<ServiceResponse<FightResultDto>>> FinalFight(FightRequestDto fight)
        {
            var result = await _service.FinalFight(fight);

            if (result == null)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("HighScore")]
        public async Task<ActionResult<ServiceResponse<List<HighScoreDto>>>> GetHighScore()
        {
            var result = await _service.GetHighScore();
            return Ok(result);
        }
    }
}
