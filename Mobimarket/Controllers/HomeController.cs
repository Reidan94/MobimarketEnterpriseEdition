using Mobimarket.BL;
using System.Web.Mvc;

namespace Mobimarket.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}