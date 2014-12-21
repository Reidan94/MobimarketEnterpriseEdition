using Mobimarket.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Mobimarket.Controllers
{
    public class ApiController : BaseController
    {
        private static string GetEnterpriseType(int type)
        {
            if (type == 1)
                return "Food and restaraunts";
            if (type == 2)
                return "Entertainment";
            return "Markets";
        }

        private string GetServiceName(int id)
        {
            if (id == 1)
                return "Advertising campaign";
            return "Preferences Analyzer";
        }

        private string GetServiceDescription(int id)
        {
            if (id == 1)
                return "Advertising campaign will help you to promote your products among customers";

            return "Preference analyzer will help to define if the product will be popular among young clients";
        }

        private string GetServiceImage(int id)
        {
            if (id == 1)
                return "/Images/advcampaign.png";
            return "/Images/prefanalyzer.png";
        }

        private void DeleteOwner()
        {
            var keyCookie = new HttpCookie("Key")
            {
                Expires = DateTime.Now.AddDays(-1)
            };

            HttpContext.Response.Cookies.Set(keyCookie);
        }

        [HttpPost]
        public JsonResult GetEnterprises()
        {
            if (!ExistsOwner())
            {
                Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                return null;
            }

            var owner = OwnerManager.DefineOwner(HttpContext);
            var enterprises = owner.Enterprises.ToList();

            Response.StatusCode = (int)HttpStatusCode.OK;

            return Json(enterprises.Select(ent => new
            {
                ent.Id,
                ent.Title,
                ent.Balance,
                ent.Type,
                ent.Description,
                ent.LogoPath
            }));
        }

        [HttpPost]
        public JsonResult GetEnterpriseContacts(int enterpriseId)
        {
            if (!ExistsOwner())
            {
                Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                return null;
            }

            var enterprise = EnterpriseManager.GetEnterprise(enterpriseId);
            return Json(enterprise.Contacts.Select(contact => new
            {
                contact.Type,
                contact.Text
            }));
        }

        [HttpPost]
        public JsonResult GetEnterpriseBranches(int enterpriseId)
        {
            if (!ExistsOwner())
            {
                Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                return null;
            }

            var enterprise = EnterpriseManager.GetEnterprise(enterpriseId);
            return Json(enterprise.Branches.Select(branch => new
            {
                branch.LogIn,
                branch.CashBoxCount,
                Adress = branch.Country + ", " + branch.City + ", " + branch.Adress
            }));
        }

        [HttpPost]
        public JsonResult GetEnterprise(int id)
        {
            if (!ExistsOwner())
            {
                Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                return null;
            }

            var enterprise = EnterpriseManager.GetEnterprise(id);
            return Json(new
            {
                enterprise.Title,
                enterprise.Description,
                StartDate = enterprise.StartDate.ToShortDateString(),
                Type = GetEnterpriseType(enterprise.Type),
                enterprise.LogoPath
            });
        }

        [HttpPost]
        public JsonResult GetTopProducts()
        {
            if (!ExistsOwner())
            {
                Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                return null;
            }

            var owner = OwnerManager.DefineOwner(HttpContext);
            var products = from ent in owner.Enterprises
                           from product in ent.Products
                           select product;

            var rnd = new Random(Environment.TickCount);

            return Json(products.Select(x => new
            {
                x.Name,
                x.Price,
                x.PicturePath,
                x.Amount,
                ordersCount = rnd.Next(30, 100)
            }).OrderBy(x => -x.ordersCount));
        }

        [HttpPost]
        public JsonResult GetTopClients()
        {
            var rnd = new Random(Environment.TickCount);

            if (!ExistsOwner())
            {
                Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                return null;
            }

            var clients = ClientManager.GetClients();
            return Json(clients.Select(x => new
            {
                name = x.AspNetUser.UserName,
                x.PicturePath,
                ordersCount = rnd.Next(30, 90)
            }).OrderBy(x => -x.ordersCount));
        }

        [HttpPost]
        public JsonResult GetEnterpriseProducts(int enterpriseId)
        {
            if (!ExistsOwner())
            {
                Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                return null;
            }

            var enterprise = EnterpriseManager.GetEnterprise(enterpriseId);
            return Json(enterprise.Products.Select(product => new
            {
                product.Name,
                product.Category,
                product.Description,
                product.Price,
                product.Amount,
                product.PicturePath
            }));
        }

        [HttpPost]
        public JsonResult GetIncome()
        {
            Random rnd = new Random(Environment.TickCount);

            List<DateTime> week = new List<DateTime>();
            foreach (var item in Enumerable.Range(0, 7))
            {
                week.Add(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - item));
            }

            return Json(week.Select(day => new
            {
                Date = day.ToShortDateString(),
                Income = rnd.Next(500, 1000)
            }));
        }

        [HttpPost]
        public JsonResult GetServices()
        {
            if (!ExistsOwner())
            {
                Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                return null;
            }

            return Json(ServiceManager.GetServices().Select(x =>
                new
                {
                    Name = GetServiceName(x.Type),
                    Description = GetServiceDescription(x.Type),
                    Duration = x.Duration + " monthes",
                    Cost = x.Cost + " $",
                    Image = GetServiceImage(x.Type)
                }));
        }

        [HttpPost]
        public void Logout()
        {
            if (!ExistsOwner())
            {
                Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                return;
            }

            DeleteOwner();
        }

        [HttpPost]
        public JsonResult UserInfo()
        {
            if (!ExistsOwner())
            {
                Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
                return null;
            }

            var owner = OwnerManager.DefineOwner(HttpContext);
            return Json(new
            {
                owner.LastName,
                owner.FirstName,
                owner.MiddleName,
                Birthday = owner.BirthDay.ToShortDateString(),
                owner.Email,
                owner.PicturePath
            });
        }
    }
}