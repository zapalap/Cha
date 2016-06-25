using Akka.Actor;
using Cha.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cha.Core.Actors
{
    public class ChannelMaster : ReceiveActor
    {
        #region Message classes

        public class ConnectUser
        {
            public ConnectUser(string userName)
            {
                UserName = userName;
            }

            public string UserName { get; private set; }
        }

        public class RecieveMessage
        {
            public RecieveMessage(string userName, string text, DateTime sendTime)
            {
                UserName = userName;
                Text = text;
                SendTime = sendTime;
            }

            public string UserName { get; private set; }
            public string Text { get; private set; }
            public DateTime SendTime { get; private set; }
        }

        public class DuplicateUser
        {
            public DuplicateUser(string userName)
            {
                UserName = userName;
            }

            public string UserName { get; private set; }
        }

        #endregion

        private HashSet<IActorRef> Users = new HashSet<IActorRef>();
        private HashSet<string> UserNames = new HashSet<string>();
        private string ChannelName;
        private IList<ChatMessage> ChatStore = new List<ChatMessage>();

        public ChannelMaster(string channelName)
        {
            ChannelName = channelName;
            Become(Open);
        }

        public void Open()
        {
            Receive<ConnectUser>(user =>
            {
                if (!UserNames.Contains(user.UserName))
                {
                    Users.Add(Context.ActorOf(Props.Create(() => new UserWorker(user.UserName)), user.UserName));
                    UserNames.Add(user.UserName);
                }
                else
                {
                    Sender.Tell(new DuplicateUser(user.UserName));
                }
            });

            Receive<RecieveMessage>(message =>
            {
                var chatMessage = new ChatMessage(message.UserName, message.SendTime, message.Text);
                ChatStore.Add(chatMessage);
                foreach (var user in Users)
                {
                    user.Tell(chatMessage);
                }
            });
        }
    }
}
