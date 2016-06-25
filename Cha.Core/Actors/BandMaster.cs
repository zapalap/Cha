using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cha.Core.Actors
{
    public class BandMaster : ReceiveActor
    {
        #region Message classes

        /// <summary>
        /// Instruct BandMaster to create a new channel
        /// </summary>
        public class CreateChannel
        {
            public CreateChannel(string name)
            {
                Name = name;
            }

            public string Name { get; private set; }
        }

        /// <summary>
        /// Instruct BandMaster to destroy a channel
        /// </summary>
        public class DestroyChannel
        {
            public DestroyChannel(IActorRef channel)
            {
                Channel = channel;
            }

            public IActorRef Channel { get; private set; }
        }

        public class GetListOfChannels
        {
            public GetListOfChannels()
            {

            }
        }

        public class ChannelList
        {
            public ChannelList(IList<string> channelNames)
            {
                ChannelNames = channelNames;
            }

            public IList<string> ChannelNames { get; private set; }
        }

        /// <summary>
        /// Reached limit of open channels and won't create new ones
        /// </summary>
        public class BandFull
        {
            public BandFull()
            {

            }
        }

        #endregion

        private HashSet<IActorRef> Channels = new HashSet<IActorRef>();
        private int ActiveChannels = 0;
        private int ChannelLimit;

        public BandMaster(int channelLimit)
        {
            ChannelLimit = channelLimit;
            Become(Open);
        }

        /// <summary>
        /// Able to open new channels
        /// </summary>
        public void Open()
        {
            Receive<CreateChannel>(create =>
            {
                Channels.Add(Context.ActorOf(Props.Create(() => new ChannelMaster(create.Name)), create.Name));
                ActiveChannels++;

                if (ActiveChannels >= ChannelLimit)
                {
                    Become(Closed);
                }
            });

            Receive<GetListOfChannels>(list => HandleListChannels());
            Receive<DestroyChannel>(destroy => HandleCloseChannel(destroy));
        }

        /// <summary>
        /// Active channel limit reached. Won't open new channels.
        /// </summary>
        public void Closed()
        {
            Receive<CreateChannel>(create =>
            {
                // Notify sender that the band is closed
                Sender.Tell(new BandFull());
            });

            Receive<GetListOfChannels>(list => HandleListChannels());
            Receive<DestroyChannel>(destroy => HandleCloseChannel(destroy));
        }

        private void HandleCloseChannel(DestroyChannel destroyChannel)
        {
            if (Channels.Contains(destroyChannel.Channel))
            {
                Channels.Remove(destroyChannel.Channel);
            }
        }

        private void HandleListChannels()
        {
            var channelNames = Channels.Select(c => c.Path.Name).ToList();
            Sender.Tell(new ChannelList(channelNames), Self);
        }
    }
}
