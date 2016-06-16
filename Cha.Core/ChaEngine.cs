using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cha.Core
{
    public class ChaEngine
    {
        private static ActorSystem Actors;

        private ChaEngine() { }

        public static ActorSystem ActorSystem
        {
            get
            {
                if (Actors == null)
                {
                    Actors = ActorSystem.Create("ChaActorSystem");
                }

                return Actors;
            }
        }
    }
}
