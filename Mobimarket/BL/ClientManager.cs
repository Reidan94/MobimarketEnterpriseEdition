using Mobimarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mobimarket.BL
{
    public class ClientManager : Manager
    {
        public static List<Client> GetClients()
        {
            return mobiMarket.Clients.ToList();
        }
    }
}