using System.ComponentModel.DataAnnotations.Schema;

namespace MessageSendApp.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public string To { get; set; }

        public string MessageBody { get; set; }

    }
}
