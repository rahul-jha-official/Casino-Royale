using Casino_Royale_Backend.RequestModels;
using Casino_Royale_Backend.ResponseModels;

namespace Casino_Royale_Backend.Interfaces;

public interface IAuthService
{
    Task<RegisterResponse> RegisterUserAsync(RegisterRequest request);
    Task<LoginResponse> LoginUserAsync(LoginRequest request);
}
