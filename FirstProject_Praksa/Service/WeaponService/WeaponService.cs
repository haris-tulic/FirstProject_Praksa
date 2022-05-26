using AutoMapper;
using FirstProject_Praksa.Database;
using FirstProject_Praksa.Dto;
using FirstProject_Praksa.Dto.Weapon;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FirstProject_Praksa.Service.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WeaponService(DataContext context, IMapper mapper,IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserID() { 
            return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        public async Task<ServiceResponse<GetWeaponDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            var response = new ServiceResponse<GetWeaponDto>();
            try
            {
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId && c.User.Id == GetUserID());
                if (character!=null)
                {
                    var weapon = _mapper.Map<Weapon>(newWeapon);
                    _context.Weapons.Add(weapon);
                    await _context.SaveChangesAsync();
                    response.Data = _mapper.Map<GetWeaponDto>(weapon);
                    response.Succees = true;
                    response.Message = "Weapon "+weapon.Name+" successfully added to " + character.Id + ". " + character.Name;
                }
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
