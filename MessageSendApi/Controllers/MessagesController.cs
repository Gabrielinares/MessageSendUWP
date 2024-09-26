using Microsoft.AspNetCore.Mvc;
using MessageSendApi.Data;
using MessageSendApi.Models;
using MessageSendApi.ViewModels;
using MessageSendApi.Helpers;
using DotNetEnv;
using MessageSendApi.Services.Interfaces;

namespace MessageSendApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {

        private readonly ILogger<MessagesController> _logger;
        private readonly IMessageService _messageService;
        private readonly ISendingService _sendingService;

        public MessagesController(ILogger<MessagesController> logger, IMessageService messageService, ISendingService sendingService)
        {
            _logger = logger;
            _messageService = messageService;
            _sendingService = sendingService;

        }

        #region Action for get messages and sending status
        [HttpGet(Name = "GetMessages")]
        public IActionResult Get()
        {
            try
            {
                var messages = _messageService.GetMessages();
                return Ok(messages);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }
        }
        #endregion

        #region Action for create messages and send trough Twilio
        [HttpPost(Name = "PostMessage")]
        public async Task<IActionResult> Post([FromBody] Message message)
        {
            try
            {
                // Add Message
                var messageCreated = await _messageService.CreateMessage(message);

                var messageTwilio = await SendMessageTwilio.sendMessage(message);

                // Save in sending table
                var sending = new Sending
                {
                    IdMessage = message.Id,
                    DateSent = messageTwilio.DateCreated.HasValue ? messageTwilio.DateCreated.Value.ToString("g") : DateTime.Now.ToString("g"),
                    ConfirmationCode = messageTwilio.Status.ToString()
                };

                var sendingCreated = await _sendingService.CreateSending(sending);

                return Ok(messageCreated);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(500);
            }

        }
        #endregion
    }
}
