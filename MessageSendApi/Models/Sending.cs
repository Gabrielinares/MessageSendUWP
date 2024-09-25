using System.ComponentModel.DataAnnotations.Schema;

namespace MessageSendApi.Models
{
    public class Sending
    {
        public int Id { get; set; }

        public int IdMessage { get; set; }
        
        public string DateSent { get; set; }

        public string ConfirmationCode { get; set; }

    }
}
