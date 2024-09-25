using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageSendApp.ViewModels
{
    internal class MessageViewModel
    {
        public string To { get; set; }
        public string Message { get; set; }
        public string DateSent { get; set; }
        public string Status { get; set; }

    }
}
