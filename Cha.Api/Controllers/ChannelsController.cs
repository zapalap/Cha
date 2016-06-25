using Akka.Actor;
using Cha.Core;
using Cha.Core.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Cha.Api.Controllers
{
    public class ChannelsController : ApiController
    {
        public async Task<IEnumerable<string>> Get()
        {
            var bandMaster = WebApiApplication.ChaEngine.BandMaster;
            var channelList = await bandMaster.Ask<BandMaster.ChannelList>(new BandMaster.GetListOfChannels());
            return channelList.ChannelNames;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public HttpResponseMessage Post([FromBody]string channelName)
        {
            var bandMaster = WebApiApplication.ChaEngine.BandMaster;
            bandMaster.Tell(new BandMaster.CreateChannel(channelName));

            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Accepted
            };
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
