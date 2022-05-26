using FirstProject_Praksa.Database;
using FirstProject_Praksa.Dto.Weapon;

namespace FirstProject_Praksa.Service.WeaponService
{
    public interface IWeaponService
    {
        public Task<ServiceResponse<GetWeaponDto>> AddWeapon(AddWeaponDto newWeapon);
    }
}
