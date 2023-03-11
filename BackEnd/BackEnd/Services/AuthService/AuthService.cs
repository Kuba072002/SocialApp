using Azure;
using Azure.Core;
using BackEnd.Data;
using BackEnd.Models;
using BackEnd.ModelsDto;
using BackEnd.Utility;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;

namespace BackEnd.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ServiceResponse<string>> Register(RegisterDto request)
        {
            if (await AuthExists(request.Email))
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "User already exists."
                };
            }
            
            Auth auth = new Auth {
                Email= request.Email
            };

            auth.Password = CreatePassword(request.Password);
            auth.SecurityStamp = Guid.NewGuid().ToString();
            var token = CreateSecurityToken("confirm_email",auth.SecurityStamp,auth.Id);

            _context.Auths.Add(auth);
            await _context.SaveChangesAsync();

            return new ServiceResponse<string> { Data = token ,Success = true, Message = "Registration successfull" };
        }

        public async Task<ServiceResponse<string>> Login(LoginDto request)
        {
            var user = await _context.Auths.FirstOrDefaultAsync(auth => auth.Email == request.Email);
            if (user == null)
            {
                return new ServiceResponse<string> { Success = false, Message = "User not exist or invalid password" };
            }

            if (user.LockoutEnd > DateTime.UtcNow)
            {
                return new ServiceResponse<string> { Success = false, Message = "User account is temporary locked" };
            }

            if (!PasswordHasher.VerifyPassword(user.Password, request.Password))
            {
                if (++user.AccessFailedCount == 5)
                {
                    user.AccessFailedCount = 0;
                    user.LockoutEnd = DateTime.UtcNow.AddHours(2);
                }
                await _context.SaveChangesAsync();

                return new ServiceResponse<string> { Success = false, Message = "User not exist or invalid password" };
            }

            if(user.VerifiedAt == null)
            {
                return new ServiceResponse<string> { Success = false, Message = "Email not verified" };
            }

            var token = CreateJwt(user);

            return new ServiceResponse<string> { Data = token, Message = " Mess", Success = true };
        }

        private async Task<bool> AuthExists(string email)
        {
            if( await _context.Auths.AnyAsync(auth => auth.Email.Equals(email)))
            {
                return true;
            }
            return false;
        }

        private string CreatePassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }
            return Convert.ToBase64String(PasswordHasher.HashMethod(password, 300000));
        }

        private string CreateJwt(Auth auth)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, auth.Id.ToString()),
                new Claim(ClaimTypes.Name, auth.Email)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private string CreateSecurityToken(string reason,string stamp,int id)
        {
            byte[] data;

            //Encrypt that bytes

            return Convert.ToBase64String(data);
        }

        public async Task<ServiceResponse> VerifySecurityToken(string token)
        {
            byte[] data = Convert.FromBase64String(token);
            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            var user = await _context.Auths.FirstOrDefaultAsync(a => a.SecurityStamp == token);
            if (user == null)
            {
                return new ServiceResponse { Success = false, Message = "Invalid token" };
            }
            user.VerifiedAt= DateTime.UtcNow;
            user.SecurityStamp = Guid.NewGuid().ToString();
            await _context.SaveChangesAsync();

            return new ServiceResponse { Success = true, Message = "Email confirmed" };
        }
    }
}
