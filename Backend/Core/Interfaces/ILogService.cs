using Backend.Core.Dtos.Log;
using System.Security.Claims;

namespace Backend.Core.Interfaces
{
    public interface ILogService
    {
        Task SaveNewLog(string UserName, string Description);
        Task<IEnumerable<GetLogDto>> GetLogAsync();
        Task<IEnumerable<GetLogDto>> GetMyLogAsync(ClaimsPrincipal User);
    }
}
