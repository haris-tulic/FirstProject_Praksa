using AutoMapper;
using FirstProject_Praksa.Database;
using FirstProject_Praksa.Dto.Characters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FirstProject_Praksa.Service.CharacterService
{
    public class CharacterService : ICharacterService
    {
        
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserID()
        {
            return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        private string GetUserRole() =>  _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        public async Task<ServiceResponse<List<GetCharacterDtos>>> AddNewCharacter(AddCharacterDtos newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDtos>>();
            var character = _mapper.Map<Character>(newCharacter);
            character.User = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == GetUserID());
            _dataContext.Add(character);
            await _dataContext.SaveChangesAsync(); 
            serviceResponse.Message = "Successfully created: " + character.Id + ". " + character.Name;
            var list = await _dataContext.Characters.Include(x=>x.User).Where(x=>x.User.Id==GetUserID()).ToListAsync();

            serviceResponse.Data = _mapper.Map<List<GetCharacterDtos>>(list);
            return serviceResponse ;
        }

      

        public async Task<ServiceResponse<List<GetCharacterDtos>>> DeleteCharacter(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDtos>>();
           
            try
            {
                var character = await _dataContext.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id==GetUserID());
                if (character!=null)
                {
                    _dataContext.Characters.Remove(character);
                    await _dataContext.SaveChangesAsync();
                    serviceResponse.Data = _mapper.Map<List<GetCharacterDtos>>(await _dataContext.Characters.Where(x=>x.User.Id==GetUserID()).ToListAsync());
                    serviceResponse.Succees = true;
                    serviceResponse.Message = "Successfully removed character: " + character.Id + ". " + character.Name;
                }
                else
                {
                    serviceResponse.Succees = false;
                    serviceResponse.Message = "Character not found!";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Succees = false;
                serviceResponse.Message = ex.Message;
            }
           
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDtos>> Get(int id)
        {
            var serviceResponse=new ServiceResponse<GetCharacterDtos>();
            try
            {
                var character =await _dataContext.Characters.Include(x=>x.Weapon).Include(x=>x.Skills).FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserID());
                serviceResponse.Data = _mapper.Map<GetCharacterDtos>(character);
                serviceResponse.Succees = true;
                serviceResponse.Message = "Successfully returned character: " + id + ". " + serviceResponse.Data.Name;
            }
            catch (Exception ex)
            {

                serviceResponse.Succees = false;
                serviceResponse.Message=ex.Message;
            }
            
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDtos>>> GetAll()
        {
            var serviceResponse=new ServiceResponse<List<GetCharacterDtos>>();
            var list = GetUserRole().Equals("Admin") ? await _dataContext.Characters.Include(c => c.User).Include(c => c.Weapon).ToListAsync()
                                                     : await _dataContext.Characters.Include(x=>x.User).Include(x=>x.Weapon).Where(x=>x.User.Id==GetUserID()).ToListAsync();
            serviceResponse.Data = _mapper.Map<List<GetCharacterDtos>>(list);
            serviceResponse.Message = "Successfully returned all characters!";
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDtos>> Update(UpdateCharacterDtos updateCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDtos>();
            try
            {
                var character =await _dataContext.Characters.Include(x=>x.User).FirstOrDefaultAsync(c => c.Id == updateCharacter.Id && c.User.Id==GetUserID());
                if (character != null)
                {
                    _mapper.Map(updateCharacter, character);
                    await _dataContext.SaveChangesAsync();
                    serviceResponse.Data = _mapper.Map<GetCharacterDtos>(character);
                    serviceResponse.Succees = true;
                    serviceResponse.Message = "Successfully changed character with ID: " + character.Id + ". " + character.Name;
                }
                else
                {
                    serviceResponse.Succees = false;
                    serviceResponse.Message = "Character can not be updated!";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Succees = false;
                serviceResponse.Message=ex.Message;
                
            }
            return serviceResponse;

        }

        public async Task<ServiceResponse<GetCharacterDtos>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
            var response = new ServiceResponse<GetCharacterDtos>();
            try
            {
                var character = await _dataContext.Characters.
                Include(x => x.User)
                .Include(x => x.Weapon)
                .Include(x=>x.Skills)
                .FirstOrDefaultAsync(x => x.Id == newCharacterSkill.CharacterId && x.User.Id == GetUserID());
                
                if (character == null)
                {
                    response.Succees = false;
                    response.Message = "Character not found!";
                    return response;
                }
                
                var skill = await _dataContext.Skills.Include(s=>s.Characters).FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
                if (skill==null)
                {
                    response.Succees = false;
                    response.Message = "Skill not found!";
                    return response;
                }
                character.Skills.Add(skill);
                await _dataContext.SaveChangesAsync();
                response.Succees = true;
                response.Data=_mapper.Map<GetCharacterDtos>(character); 
            }
            catch (Exception ex)
            {
                response.Succees = false;
                response.Message=ex.Message;
            }
            return response;
        }
    }
}
