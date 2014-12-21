using Mobimarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mobimarket.BL
{
    public class ServiceManager : Manager
    {
        public static List<Service> GetServices()
        {
            return mobiMarket.Services.ToList();
        }
    }
}