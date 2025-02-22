using CadSimulation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadSimulation.remoteStore
{
    public class RemoteStoreConfiguration: IConfigurationForStore
    {
        public string URI { get; set; }
        public RemoteStoreConfiguration(string sRemoteUri)
        {
            this.URI = sRemoteUri;
        }

    }
}
