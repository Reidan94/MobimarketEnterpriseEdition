using Mobimarket.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mobimarket.Controllers
{
    public class BaseController : Controller
    {
        protected bool ExistsOwner() 
        {
            return OwnerManager.DefineOwner(HttpContext) != null;
        }
	}
}