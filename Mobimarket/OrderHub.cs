using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Mobimarket
{
    public class OrderHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.hello(name, message);
        }
    }
}