using MessageSendApi.Data;
using MessageSendApi.Repository.Interfaces;
using MessageSendApi.ViewModels;
using MessageSendApi.Models;

namespace MessageSendApi.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;

        public MessageRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<MessagesInfoViewModel> GetMessages()
        {
            return _context.Messages
                .Join(_context.Sendigs,  m => m.Id, s => s.IdMessage, (m, s) => new MessagesInfoViewModel
                    {
                        To = m.To,
                        Message = m.MessageBody,
                        DateSent = s.DateSent,
                        Status = s.ConfirmationCode
                    }).ToList();
        }

        public async Task<Message> CreateMessage(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }
    }
}
