using Backend.Core.Dtos.General;
using Backend.Core.Dtos.Message;
using System.Security.Claims;

namespace Backend.Core.Interfaces
{
    public interface IMessageService
    {
        Task<GeneralServiceResponseDto> CreateNewMessageAsync(ClaimsPrincipal User,CreateMessageDto createMessageDto);
        Task<IEnumerable<GetMessageDto>> GetMessageAsync();
        Task<IEnumerable<GetMessageDto>> GetMyMessageAsync(ClaimsPrincipal User);
    }
}
