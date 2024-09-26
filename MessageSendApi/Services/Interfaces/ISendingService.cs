using MessageSendApi.Models;

namespace MessageSendApi.Services.Interfaces
{
    public interface ISendingService
    {
        Task<Sending> CreateSending(Sending sending);
    }
}
