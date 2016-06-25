using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cha.Core.Entities
{
    public class ChatMessage
    {
        public ChatMessage(string sender, DateTime time, string text)
        {
            Sender = sender;
            Time = time;
            Text = text;
        }

        public string Sender { get; private set; }
        public DateTime Time { get; private set; }
        public string Text { get; private set; }
    }
}
