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
        private static bool HasOrder = false;
        //private static Dictionary<int, Queue<OrderModel>> orders = new Dictionary<int, Queue<OrderModel>>();
        private static OrderModel order;
        private static DateTime curDateTime;
        private static bool IsFinished = false;

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

        public void OrderCame()
        {
            HasOrder = true;
        }

        [HttpPost]
        public JsonResult OrderInfo()
        {
            if (HasOrder)
            {
                HasOrder = false;
                return Json(true);
            }

            return Json(false);
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

        [HttpGet]
        public void Start(int hour, int minutes)
        {
            IsFinished = false;
            order = new OrderModel();
            curDateTime = new DateTime(2014, 12, 23, hour, minutes, 0);
        }

        [HttpGet]
        public JsonResult IsReady()
        {
            if (IsFinished)
            {
                IsFinished = false;
                return Json(new { IsReady = true }, JsonRequestBehavior.AllowGet);
            }
            else return Json(new { IsReady = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AddOrder(int id)
        {
            var product = ProductManager.GetProduct(id);
            order.orderItems.Add(new OrderItem
            {
                Id = id,
                Price = product.Price
            });

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Finish()
        {
            order = new OrderModel();
            IsFinished = true;
            return Json(new
            {
                CashBox = EnterpriseManager.GetFreeCashBox(EnterpriseManager.CurrentEnterprise()),
                dateTime = curDateTime
            }, JsonRequestBehavior.AllowGet);


        }

        public void ConfirmOrder(int id)
        {
            EnterpriseManager.ConfirmOrder(id);
        }

        [HttpGet]
        public JsonResult GetEnterprise()
        {
            var enterprise = EnterpriseManager.GetEnterprise(1);
            return Json(new
            {
                enterprise.Title,
                enterprise.Description,
                enterprise.LogoPath
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOrder(int entId)
        {
            var myorders = new List<object>();
            foreach (var item in order.orderItems)
            {
                var product = ProductManager.GetProduct(item.Id);

                myorders.Add(new
                {
                    Name = product.Name,
                    Image = product.PicturePath,
                    Price = product.Price
                });
            }

            return Json(myorders, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsOrderConfirmed(int id)
        {
            return Json(EnterpriseManager.CheckOrder(id));
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