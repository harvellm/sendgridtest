using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Text;

namespace testing
{
    public class FileHub : Hub
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
        public void SaveFile(string fileName, string fileData)
        {
            string name = Context.QueryString["name"];
            byte[] file = System.Convert.FromBase64String(fileData);
            System.IO.File.WriteAllBytes(@"\\ver-fileserver\Veritas\Test\Temp\verivdp testing\" + fileName, file);

            foreach(var connectionId in _connections.GetConnections(name))
            {
                Clients.Client(connectionId).writeFile(name, "File saved");
            }
        }
    }
}