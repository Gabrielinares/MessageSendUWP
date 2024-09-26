using MessageSendApi.Models;
using MessageSendApi.Repository.Interfaces;
using MessageSendApi.Services.Interfaces;

namespace MessageSendApi.Services
{
    public class SendingService : ISendingService
    {
        private readonly ISendingRepository _sendingRepository;

        public SendingService(ISendingRepository sendingRepository)
        {
            _sendingRepository = sendingRepository;
        }

        public async Task<Sending> CreateSending(Sending sending)
        {
            return await _sendingRepository.CreateSending(sending);
        }
    }
}
