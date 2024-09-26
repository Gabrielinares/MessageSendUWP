using MessageSendApi.Data;
using MessageSendApi.Models;
using MessageSendApi.Repository.Interfaces;

namespace MessageSendApi.Repository
{
    public class SendingRepository : ISendingRepository
    {
        private readonly AppDbContext _context;

        public SendingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Sending> CreateSending(Sending sending)
        {
            _context.Sendigs.Add(sending);
            await _context.SaveChangesAsync();
            return sending;
        }
    }
}
