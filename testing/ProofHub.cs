using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace testing
{
    public class ProofHub : Hub
    {
        #region Connections
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();

        public override Task OnConnected()
        {
            string name = Context.QueryString["name"];

            _connections.Add(name, Context.ConnectionId);

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.QueryString["name"];

            _connections.Remove(name, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = Context.QueryString["name"];

            if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
            {
                _connections.Add(name, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
        #endregion

        public void GenerateProof(string project, string jobId, string jobName)
        {
            string name = Context.QueryString["name"];
            DateTime start = DateTime.Now;
            bool Worked = false;
            if(VerivdpCalls.GenerateProof(project,jobId,jobName))
            {
                Worked = VerivdpCalls.GenerateSuccess(jobId,start);
            }

            foreach (var connectionId in _connections.GetConnections(name))
            {
                Clients.Client(connectionId).proofMessage(name, Worked ? string.Format( "cart {0} Proof is ready.",jobId) : string.Format( "cart {0} No proof generated.",jobId));
            }
        }
    }
}