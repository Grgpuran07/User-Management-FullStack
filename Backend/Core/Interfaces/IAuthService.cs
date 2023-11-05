using Backend.Core.Dtos.Auth;
using Backend.Core.Dtos.General;
using System.Security.Claims;

namespace Backend.Core.Interfaces
{
    public interface IAuthService
    {
        Task<GeneralServiceResponseDto> SeedRolesAsync();
        Task<GeneralServiceResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<LoginServiceResponseDto?> LoginAsync(LoginDto loginDto);
        Task<GeneralServiceResponseDto> UpdateRoleAsync(ClaimsPrincipal User,UpdateRoleDto updateRoleDto);
        Task<LoginServiceResponseDto?> MeAsync(MeDto meDto);
        Task<IEnumerable<UserInfoResult>> GetUserListAsync();
        Task<UserInfoResult?> GetUserDetailsByUserNameAsync(string userName);
        Task<IEnumerable<string>> GetUsersNameListAsync();
    }
}
