using System.Web.Mvc;

namespace Mobimarket.Controllers
{
    using System.Web;

    using BL;
    using Models;
    using Mobimarket.CrosscuttingFunctionality.ErrorProcessing;
    using Mobimarket.CrosscuttingFunctionality.Security;
    using System;

    public class OwnerController : BaseController
    {
        public ActionResult Registrate(int? id)
        {
            if (id != null)
                ViewBag.Result = ProcessResults.GetById(id.Value);
            return View();
        }

        public ActionResult Confirm(string hash)
        {
            ProcessResult pr = OwnerManager.ConfirmRegistration(hash);
            return RedirectToAction("Dashboard", "Index", new { id = pr.Id });
        }

        private void SetOwner(string hashedKey)
        {
            var cookieKey = new HttpCookie("Key")
            {
                Value = hashedKey,
                Expires = DateTime.MaxValue
            };

            HttpContext.Response.Cookies.Remove("Key");
            HttpContext.Response.SetCookie(cookieKey);
        }

        public ActionResult LogIn(int? id)
        {
            if (id != null)
                ViewBag.Result = ProcessResults.GetById(id.Value);
            return View();
        }

        [HttpPost]
        public ActionResult ManageLogIn(string email, string password)
        {
            Owner owner;
            var result = OwnerManager.LogIn(email, password, out owner);
            if (result == ProcessResults.SuccessfullyLoggedIn)
            {
                string hashedKey = SecurityManager.GetHashString(owner.BirthDay + owner.FirstName + owner.Email);
                SetOwner(hashedKey);
                return RedirectToAction("Index", "Dashboard");
            }

            return RedirectToAction("LogIn", new { id = result.Id });
        }

        [HttpPost]
        public ActionResult ManageRegistration(Owner owner,
            HttpPostedFileBase imageUpload,
            string ConfirmPassword)
        {
            if (owner.Password != ConfirmPassword) return null;
            var result = OwnerManager.RegistrateOwner(HttpContext, owner, imageUpload);
            return RedirectToAction("Registrate", new { id = result.Id });
        }

        public ActionResult Enterprises()
        {
            if (!ExistsOwner())
                return RedirectToAction("LogIn", "Owner");
            var curOwner = OwnerManager.DefineOwner(HttpContext);
            return View(curOwner);
        }
    }
}