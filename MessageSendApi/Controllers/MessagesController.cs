using Microsoft.AspNetCore.Mvc;
using MessageSendApi.Data;
using MessageSendApi.Models;
using MessageSendApi.ViewModels;
using MessageSendApi.Helpers;
using DotNetEnv;

namespace MessageSendApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {

        private readonly ILogger<MessagesController> _logger;
        private readonly AppDbContext _context;

        public MessagesController(ILogger<MessagesController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;

        }

        #region Action for get messages and sending status
        [HttpGet(Name = "GetMessages")]
        public IActionResult Get()
        {
            try
            {
                var messages = _context.Messages
                                .Join(_context.Sendigs,
                                    m => m.Id,
                                    s => s.IdMessage,
                                    (m, s) => new MessagesInfoViewModel
                                    {
                                        To = m.To,
                                        Message = m.MessageBody,
                                        DateSent = s.DateSent,
                                        Status = s.ConfirmationCode
                                    }).ToList();

                return new OkObjectResult(messages);
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
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();

                var messageTwilio = await SendMessageTwilio.sendMessage(message);

                // Save in sending table
                var sending = new Sending
                {
                    IdMessage = message.Id,
                    DateSent = messageTwilio.DateCreated.HasValue? messageTwilio.DateCreated.Value.ToString("g") : DateTime.Now.ToString("g"),
                    ConfirmationCode = messageTwilio.Status.ToString()
                };

                _context.Sendigs.Add(sending);
                await _context.SaveChangesAsync();

                return new OkObjectResult(message);
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
