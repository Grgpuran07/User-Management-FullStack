using Backend.Core.DbContext;
using Backend.Core.Dtos.General;
using Backend.Core.Dtos.Message;
using Backend.Core.Entities;
using Backend.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Backend.Core.Services
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogService _logService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessageService(ApplicationDbContext context, ILogService logService,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logService = logService;
            _userManager = userManager;
        }
        public async Task<GeneralServiceResponseDto> CreateNewMessageAsync(ClaimsPrincipal User, CreateMessageDto createMessageDto)
        {
            if (User.Identity.Name == createMessageDto.ReceiverUserName)
            {
                return new GeneralServiceResponseDto { 
                  IsSucceed = false,
                  StatusCode = 400,
                  Message = "Sender and Receiver can not be same."
                };
            }

            var isReceiverUserNameValid = _userManager.Users.Any(q=>q.UserName==createMessageDto.ReceiverUserName);
            if (!isReceiverUserNameValid) { 
              return new GeneralServiceResponseDto
              {
                  IsSucceed = false,
                  StatusCode = 400,
                  Message = "Receiver UserName is not valid."
              };
            }

            Message newMessage = new Message()
            {
                SenderUserName = User.Identity.Name,
                ReceiverUserName = createMessageDto.ReceiverUserName,
                Text = createMessageDto.Text
            };

            await _context.Messages.AddAsync(newMessage);
            await _context.SaveChangesAsync();
            await _logService.SaveNewLog(User.Identity.Name,"Send Message");

           
            return new GeneralServiceResponseDto
            {
                IsSucceed = true,
                StatusCode = 201,
                Message = "Message saved sucessfully."
            };
        }

        public async Task<IEnumerable<GetMessageDto>> GetMessageAsync()
        {
            var messages = await _context
                .Messages
                .Select(m => new GetMessageDto()
                {
                    Id = m.id,
                    SenderUserName = m.SenderUserName,
                    ReceiverUserName = m.ReceiverUserName,
                    Text = m.Text,
                    CreatedAt = m.CreatedAt,
                })
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();
            return messages;
        }

        public async Task<IEnumerable<GetMessageDto>> GetMyMessageAsync(ClaimsPrincipal User)
        {
            var loggedInUser = User.Identity.Name;
            var messages = await _context
                .Messages
                .Where(m=>m.SenderUserName==loggedInUser || m.ReceiverUserName == loggedInUser)
                .Select(m => new GetMessageDto()
                {
                    Id = m.id,
                    SenderUserName = m.SenderUserName,
                    ReceiverUserName = m.ReceiverUserName,
                    Text = m.Text,
                    CreatedAt = m.CreatedAt,
                })
                .OrderByDescending(q => q.CreatedAt)
                .ToListAsync();
            return messages;
        }
    }
}
