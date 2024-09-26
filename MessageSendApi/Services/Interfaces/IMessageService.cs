using MessageSendApi.ViewModels;
using MessageSendApi.Models;

namespace MessageSendApi.Services.Interfaces
{
    public interface IMessageService
    {
        List<MessagesInfoViewModel> GetMessages();
        Task<Message> CreateMessage(Message message);
    }
}
