using Backend.Core.Constants;
using Backend.Core.Dtos.Message;
using Backend.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        //Route -> Create a new message to send to another user
        [HttpPost]
        [Route("create")]
        [Authorize]
        public async Task<IActionResult> CreateNewMessage([FromBody] CreateMessageDto createMessageDto)
        {
            var result = await _messageService.CreateNewMessageAsync(User,createMessageDto);
            if (result.IsSucceed) {
                return Ok(result.Message);
            }

            return StatusCode(result.StatusCode,result.Message);
        }

        //Route -> Get all message for user either sender or receiver
        [HttpGet]
        [Route("mine")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetMessageDto>>> GetMyMessages()
        {
            var messages = await _messageService.GetMyMessageAsync(User);
            return Ok(messages);
        }

        //Route -> Get all messages with owner and admin access
        [HttpGet]
        [Authorize(Roles = StaticUserRoles.OwnerAdmin)]
        public async Task<ActionResult<IEnumerable<GetMessageDto>>> GetAllMessages()
        {
            var messages = await _messageService.GetMyMessageAsync(User);
            return Ok(messages);
        }
    }
}
