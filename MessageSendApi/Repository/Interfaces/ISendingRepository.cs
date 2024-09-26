using MessageSendApi.Models;

namespace MessageSendApi.Repository.Interfaces
{
    public interface ISendingRepository
    {
        Task<Sending> CreateSending(Sending sending);

    }
}
