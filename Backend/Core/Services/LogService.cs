using Backend.Core.DbContext;
using Backend.Core.Dtos.Log;
using Backend.Core.Entities;
using Backend.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Backend.Core.Services
{
    public class LogService : ILogService
    {
        private readonly ApplicationDbContext _context;
        public LogService(ApplicationDbContext context) { 
           _context = context;
        }

        public async Task SaveNewLog(string UserName, string Description)
        {
            var newLog = new Log()
            {
                UserName = UserName,
                Description = Description
            };
            await _context.Logs.AddAsync(newLog);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<GetLogDto>> GetLogAsync()
        {
             var logs = await _context.Logs
                .Select(x => new GetLogDto(){ UserName = x.UserName, Description = x.Description,CreatedAt=x.CreatedAt })
                .OrderByDescending(q=>q.CreatedAt).ToListAsync();
            return logs;
        }

        public async Task<IEnumerable<GetLogDto>> GetMyLogAsync(ClaimsPrincipal User)
        {
            var logs = await _context.Logs
                .Where(q=>q.UserName==User.Identity.Name)
               .Select(x => new GetLogDto() { UserName = x.UserName, Description = x.Description, CreatedAt = x.CreatedAt })
               .OrderByDescending(q => q.CreatedAt).ToListAsync();
            return logs;
        }
    }
}
