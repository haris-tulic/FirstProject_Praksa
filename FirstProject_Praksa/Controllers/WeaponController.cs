using FirstProject_Praksa.Database;
using FirstProject_Praksa.Dto.Weapon;
using FirstProject_Praksa.Service.WeaponService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject_Praksa.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WeaponController : ControllerBase
    {
        private readonly IWeaponService _service;

        public WeaponController(IWeaponService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetWeaponDto>>> Add(AddWeaponDto newWeapon)
        {
            var response=await _service.AddWeapon(newWeapon);
            if (response.Data==null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
