using FirstProject_Praksa.Authentification;
using FirstProject_Praksa.Database;
using FirstProject_Praksa.Dto.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject_Praksa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _service;

        public AuthController(IAuthRepository service)
        {
            _service = service;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(RegisterUser user)
        {
            var response = await _service.Register(
                new User { UserName = user.UserName }, user.Password
                );
            if (!response.Succees)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(LoginUser user)
        {
            var response = await _service.Login(user.UserName, user.Password);
            if (!response.Succees)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
          
    }
}
