using MessageSendApi.ViewModels;
using MessageSendApi.Repository.Interfaces;
using MessageSendApi.Services.Interfaces;
using MessageSendApi.Models;

namespace MessageSendApi.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public List<MessagesInfoViewModel> GetMessages() {
            return _messageRepository.GetMessages();
        }
        public async Task<Message> CreateMessage(Message message)
        {
            return await _messageRepository.CreateMessage(message);
        }
    }
}
