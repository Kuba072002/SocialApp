using BackEnd.ModelsDto;
using BackEnd.Utility;

namespace BackEnd.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<string>> Register(RegisterDto request);
        Task<ServiceResponse<string>> Login(LoginDto request);
        Task<ServiceResponse> VerifySecurityToken(string purpose, string token);
    }
}
