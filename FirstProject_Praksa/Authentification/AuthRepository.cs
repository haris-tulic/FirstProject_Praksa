using FirstProject_Praksa.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FirstProject_Praksa.Authentification
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public AuthRepository(DataContext context,IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(x=> x.UserName.Equals(username));
            if (user==null)
            {
                response.Succees = false;
                response.Message = "User doesn't exist!";
            }
            else if (!VerifyPasswordHash(password,user.PasswordHash,user.PasswordSalt))
            {
                response.Succees = false;
                response.Message = "Invallid password!";
            }
            else
            {
                response.Succees= true;
                response.Data = CreateToken(user);
            }
            return response;
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var response = new ServiceResponse<int>();
            if (await UserExist(user.UserName))
            {
                response.Succees = false;
                response.Message = "User already exist!";
                return response;
            }
            
            GenerateHashPassword(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            response.Data = user.Id;
            return response;
                
                
        }

        public async Task<bool> UserExist(string username)
        {
            if (await _context.Users.AnyAsync(x=>x.UserName.ToLower().Equals(username.ToLower())))
            {
                return true;
            }
            return false;
        }
        private void GenerateHashPassword(string password,out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac=new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        // saljemo password u plain textu, hash i salt; prosljedjujemo salt u HMACSHA512 klasu; saljemo passworde iz database osim password-a kojeg unosi korisnik
        //uporedjujemo postojeci Hash sa novim stvorenim
        private bool VerifyPasswordHash(string password,  byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) 
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
