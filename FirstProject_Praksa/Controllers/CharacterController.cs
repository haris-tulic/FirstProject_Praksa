using FirstProject_Praksa.Database;
using FirstProject_Praksa.Dto;
using FirstProject_Praksa.Service.CharacterService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject_Praksa.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _service;
        public CharacterController(ICharacterService service)
        {
            _service = service;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDtos>>>Get(int id)
        {
            var response = await _service.Get(id);
            if (response.Data==null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDtos>>>> GetAll()
        {
            return Ok(await _service.GetAll());
        }
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDtos>>>> AddNewCharacter(AddCharacterDtos character)
        {
            return Ok(await _service.AddNewCharacter(character));
        }
        [HttpPut]
        public async Task<ActionResult<ServiceResponse<GetCharacterDtos>>> Update(UpdateCharacterDtos updateCharacter)
        {
            var response =await _service.Update(updateCharacter);
            if (response.Data==null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDtos>>>> Delete(int id)
        {
            var response = await _service.DeleteCharacter(id);
            if (response.Data==null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
