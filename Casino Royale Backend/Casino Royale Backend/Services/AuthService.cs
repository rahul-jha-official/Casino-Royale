using Casino_Royale_Backend.Constants;
using Casino_Royale_Backend.Entities;
using Casino_Royale_Backend.Interfaces;
using Casino_Royale_Backend.RequestModels;
using Casino_Royale_Backend.ResponseModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Casino_Royale_Backend.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly string _authSigningKey;


    public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _authSigningKey = configuration.GetValue<string>("AuthSetting:SecretKey") ?? "";
    }

    public async Task<RegisterResponse> RegisterUserAsync(RegisterRequest request)
    {
        var response = new RegisterResponse
        {
            Email = request.Email,
            FullName = request.Name,
            IsSuccess = false,
            ErrorMessage = string.Empty
        };

        try
        {
            if (request is null || 
                string.IsNullOrWhiteSpace(request.Name) || 
                string.IsNullOrWhiteSpace(request.Email) || 
                string.IsNullOrWhiteSpace(request.Password))
            {
                response.ErrorMessage = "Invalid Data";
                return response;
            }
            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.Name
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            await CreateRoleAsync();
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, AppConstants.USER_ROLE);
                response.ErrorMessage = string.Empty;
                response.IsSuccess = true;
            }
            else
            {
                response.ErrorMessage = result.Errors.Select(e => e.Description).LastOrDefault() ?? string.Empty;
            }
            return response;
        }
        catch(Exception ex)
        {
            response.ErrorMessage = ex.Message;
            return response;
        }
    }

    public async Task<LoginResponse> LoginUserAsync(LoginRequest request)
    {
        var response = new LoginResponse
        {
            FullName = string.Empty,
            Email = request.Email,
            Token = string.Empty,
            Role = string.Empty,
            ErrorMessage = "Invalid Credentials",
            IsSuccess = false
        };

        try
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return response;
            }
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return response;
            }
            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            
            if (!result)
            {
                return response;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_authSigningKey);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(
                    [
                    new ("fullname",user.FullName),
                    new ("id",user.Id),
                    new (ClaimTypes.Email,user.Email!.ToString()),
                    new (ClaimTypes.Role, _userManager.GetRolesAsync(user).Result.FirstOrDefault()!)
                    ]),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            response.FullName = user.FullName;
            response.Token = tokenHandler.WriteToken(token);
            response.Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault() ?? string.Empty;
            response.ErrorMessage = string.Empty;
            response.IsSuccess = true;

            return response; 
        }
        catch (Exception)
        {
            return response;
        }
    }

    private async Task CreateRoleAsync()
    {
        if (!await _roleManager.RoleExistsAsync(AppConstants.ADMIN_ROLE))
        {
            await _roleManager.CreateAsync(new IdentityRole(AppConstants.ADMIN_ROLE));
            await _roleManager.CreateAsync(new IdentityRole(AppConstants.USER_ROLE));
        }
    }
}
