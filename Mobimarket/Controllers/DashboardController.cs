using Mobimarket.BL;
using Mobimarket.CrosscuttingFunctionality.ErrorProcessing;
using Mobimarket.CrosscuttingFunctionality.Security;
using Mobimarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mobimarket.Controllers
{
    public class DashboardController : BaseController
    {
        //
        // GET: /Dashboard/
        public ActionResult Index(int? id)
        {
            if (!ExistsOwner()) return RedirectToAction("LogIn", "Owner");
            if (id != null)
                ViewBag.Result = ProcessResults.GetById(id.Value);

            return View();
        }

        public ActionResult Enterprise(int id)
        {
            if (!ExistsOwner())
                return RedirectToAction("LogIn", "Owner");

            var enterprise = EnterpriseManager.GetEnterprise(id);
            if (enterprise == null)
                return RedirectToAction("Enterprises", "Owner");

            return View(enterprise);
        }

        public ActionResult AddBranch(int enterpriseId, int? id)
        {
            if (id != null)
                ViewBag.Result = ProcessResults.GetById(id.Value);
            ViewBag.EntId = enterpriseId;
            return View();
        }

        [HttpPost]
        public ActionResult ManageEnterpriseAdding(Enterprise enterprise, HttpPostedFileBase imageUpload)
        {
            var owner = OwnerManager.DefineOwner(HttpContext);
            enterprise.OwnerId = owner.Id;
            var result = EnterpriseManager.AddEnterprise(enterprise, imageUpload);
            return RedirectToAction("AddEnterprise", new { id = result.Id });
        }

        [HttpPost]
        public ActionResult ManageContactAdding(Contact contact)
        {
            var cType = (ContactType)contact.Type;
            if (!Validator.ValidateContact(contact.Text, cType))
                return RedirectToAction("AddContact", new
                {
                    enterpriseId = contact.EnterpriseId,
                    id = ProcessResults.InvalidContact.Id
                });

            EnterpriseManager.AddEnterpriseContact(contact);

            return RedirectToAction("AddContact", new
            {
                enterpriseId = contact.EnterpriseId,
                id = ProcessResults.ContactsSuccessfullyAdded.Id
            });
        }

        [HttpPost]
        public ActionResult ManageBranchAdding(Branch branch)
        {
            var result = EnterpriseManager.AddBranch(branch);
            return RedirectToAction("AddBranch", new { enterpriseId = branch.EnterpriseId, id = result.Id });
        }

        [HttpPost]
        public ActionResult ManageProductAdding(Product product, HttpPostedFileBase imageUpload)
        {
            var res = EnterpriseManager.AddProduct(product, imageUpload);
            return RedirectToAction("AddProduct", new { enterpriseId = product.EnterpriseId, id = res.Id });
        }

        public ActionResult AddEnterprise(int? id)
        {
            if (id != null)
                ViewBag.Result = ProcessResults.GetById(id.Value);
            return View();
        }

        public ActionResult AddContact(int enterpriseId, int? id)
        {
            if (id != null)
                ViewBag.Result = ProcessResults.GetById(id.Value);
            ViewBag.EntId = enterpriseId;
            return View();
        }

        public ActionResult AddProduct(int enterpriseId, int? id)
        {
            if (id != null)
                ViewBag.Result = ProcessResults.GetById(id.Value);
            ViewBag.EntId = enterpriseId;

            return View();
        }

        public ActionResult TopClients()
        {
            return View();
        }

        public ActionResult GetServices() 
        {
            return View();
        }

        public ActionResult TopProducts() 
        {
            return View();
        }

        public ActionResult GetIncome() 
        {
            return View();
        }

        public ActionResult UserInfo()
        {
            return View();
        }
    }
}