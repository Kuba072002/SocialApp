using Azure;
using Azure.Core;
using BackEnd.Data;
using BackEnd.Models;
using BackEnd.ModelsDto;
using BackEnd.Services.UserService;
using BackEnd.Utility;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Buffers.Binary;
using System.Collections.Generic;
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
        private readonly IDataProtector _dataProtector;

        public AuthService(DataContext context, IConfiguration configuration, IDataProtectionProvider dataProtectionProvider)
        {
            _context = context;
            _configuration = configuration;
            _dataProtector = dataProtectionProvider.CreateProtector(_configuration.GetSection("AppSettings:KeyProtection").Value);
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

            User user = new User(request.FirstName,request.LastName,request.BirthDate);
            auth.UserId = user.Id;
            auth.User = user;
            if (request.Picture != null)
            {
                if (request.Picture.Length > 0)
                {
                    var picture = new Picture();
                    picture.Create(request.Picture);
                    _context.Pictures.Add(picture);
                    user.PictureId = picture.Id;
                    user.Picture = picture;
                }
            }

            _context.Users.Add(user);
            _context.Auths.Add(auth);
            await _context.SaveChangesAsync();

            var token = CreateSecurityToken("confirm_email",auth.SecurityStamp,auth.Id);

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
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
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

        private string CreateSecurityToken(string purpose,string stamp,int id)
        {
            var dateBytes = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            var stampBytes = new Guid(stamp).ToByteArray();
            var idBytes = BitConverter.GetBytes(id);
            var purposeBytes = Encoding.ASCII.GetBytes(purpose);

            byte[] data = new byte[8 + 16 + 4 + purposeBytes.Length];
            
            Buffer.BlockCopy(dateBytes, 0, data, 0, 8);
            Buffer.BlockCopy(stampBytes, 0, data, 8, 16);
            Buffer.BlockCopy(idBytes, 0, data, 24, 4);
            Buffer.BlockCopy(purposeBytes, 0, data, 28, purposeBytes.Length);

            return Convert.ToBase64String(_dataProtector.Protect(data));
        }

        public async Task<ServiceResponse> VerifySecurityToken(string purpose,string token)
        {
            byte[] data = _dataProtector.Unprotect(Convert.FromBase64String(token));
          
            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            
            if(when.AddDays(1) < DateTime.UtcNow)
            {
                return new ServiceResponse { Success = false, Message = "Token expired" };
            }

            byte[] purposeBytes = new byte[data.Length - 28];
            Buffer.BlockCopy(data, 28, purposeBytes, 0, data.Length - 28);

            string purposeFromTocken = Encoding.ASCII.GetString(purposeBytes);

            if (!purposeFromTocken.Equals(purpose))
            {
                return new ServiceResponse { Success = false, Message = "Purposes not equals" };
            }

            byte[] stampBytes = new byte[16];
            Buffer.BlockCopy(data, 8, stampBytes, 0, 16);
            string stamp = new Guid(stampBytes).ToString();
            var id = BitConverter.ToInt32(data, 24);
            
            var user = await _context.Auths.FirstOrDefaultAsync(a => a.Id == id);
            if (user == null)
            {
                return new ServiceResponse { Success = false, Message = "Invalid token" };
            }
            if (!user.SecurityStamp.Equals(stamp))
            {
                return new ServiceResponse { Success = false, Message = "Invalid token" };
            }
            if (purpose.Equals("confirm_email")) { 
                user.VerifiedAt= DateTime.UtcNow;
                user.SecurityStamp = Guid.NewGuid().ToString();
                await _context.SaveChangesAsync();

                return new ServiceResponse { Success = true, Message = "Email confirmed" };
            }
            return new ServiceResponse { Success = false, Message = "Invalid token" };
        }
    }
}
