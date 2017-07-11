using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Threading;

namespace testing
{
    public class ChatHub : Hub
    {
        private readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();

        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }

        public void Send(string name, string message, string to)
        {
            foreach (var connectionId in _connections.GetConnections(to))
            {
                Clients.Client(connectionId).broadcastMessage(name, message);
            }
        }

        public void GetCartStatus(string name)
        {
            StatusChecker.CheckPfStatus();

            foreach (var connectionId in _connections.GetConnections(name))
            {
                Clients.Client(connectionId).broadcastMessage(name, "Proof is ready");
            }
        }

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
    }
}