using AutoMapper;
using FirstProject_Praksa.Database;
using FirstProject_Praksa.Dto;
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
                var character = await _dataContext.Characters.FirstOrDefaultAsync(c => c.Id == id);
                 _dataContext.Characters.Remove(character);
                await _dataContext.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<List<GetCharacterDtos>>(await _dataContext.Characters.ToListAsync());
                serviceResponse.Succees = true;
                serviceResponse.Message = "Successfully removed character: " + character.Id + ". " + character.Name;
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
                var character =await _dataContext.Characters.FirstOrDefaultAsync(c => c.Id == id);
                //var character = await _dataContext.Characters.Where(x => x.User.Id == GetUSerID).ToList();
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
            var list=await _dataContext.Characters.Include(x=>x.User).Where(x=>x.User.Id==GetUserID()).ToListAsync();
            serviceResponse.Data = _mapper.Map<List<GetCharacterDtos>>(list);
            serviceResponse.Message = "Successfully returned all characters!";
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDtos>> Update(UpdateCharacterDtos updateCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDtos>();
            try
            {
                var character =await _dataContext.Characters.FirstOrDefaultAsync(c => c.Id == updateCharacter.Id);
                _mapper.Map(updateCharacter, character);
                await _dataContext.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetCharacterDtos>(character);
                serviceResponse.Succees = true;
                serviceResponse.Message = "Successfully changed character with ID: " + character.Id + ". " + character.Name;

            }
            catch (Exception ex)
            {
                serviceResponse.Succees = false;
                serviceResponse.Message=ex.Message;
                
            }
            return serviceResponse;

        }
    }
}
