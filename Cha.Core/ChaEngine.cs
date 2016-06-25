using Akka.Actor;
using Cha.Core.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cha.Core
{
    public class ChaEngine
    {
        public ActorSystem Actors { get; private set; }
        public IActorRef BandMaster { get; private set; } 

        public ChaEngine()
        {
            Actors = ActorSystem.Create("ChaActorSystem");
            BandMaster = Actors.ActorOf(Props.Create(() => new BandMaster(10)), "bandMaster");
        }
    }
}
