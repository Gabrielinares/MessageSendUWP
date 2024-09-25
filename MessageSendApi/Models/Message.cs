using System.ComponentModel.DataAnnotations.Schema;

namespace MessageSendApi.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public string To { get; set; }

        public string MessageBody { get; set; }

    }
}
