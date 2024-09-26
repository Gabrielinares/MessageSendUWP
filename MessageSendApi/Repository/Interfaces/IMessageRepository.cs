using MessageSendApi.ViewModels;
using MessageSendApi.Models;

namespace MessageSendApi.Repository.Interfaces
{
    public interface IMessageRepository
    {
        List<MessagesInfoViewModel> GetMessages();
        Task<Message> CreateMessage(Message message);
    }
}
