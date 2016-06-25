using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cha.Core.Actors
{
    public class UserWorker : ReceiveActor
    {
        private string UserName;

        public UserWorker(string userName)
        {
            UserName = userName;
        }
    }
}
