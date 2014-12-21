using System.IO;
using System.Web;

using Mobimarket.Models;
using System.Web.Hosting;

namespace Mobimarket.BL
{
    public abstract class Manager
    {
        public static MobimarketDBEntities mobiMarket;

        static Manager()
        {
            mobiMarket = new MobimarketDBEntities();
        }

        private static string SaveImageBase(HttpPostedFileBase img, string path)
        {
            img.SaveAs(path);
            return path;
        }

        protected static string SaveProductImage(HttpPostedFileBase imageUpload, string name)
        {
            var res = name + Path.GetExtension(imageUpload.FileName);
            var path = HostingEnvironment.MapPath(MvcApplication.ProductsImage) + res;
            SaveImageBase(imageUpload, path);
            return res;
        }

        protected static string SaveUserImage(HttpPostedFileBase imageUpload, string name)
        {
            var res = name + Path.GetExtension(imageUpload.FileName);
            var path = HostingEnvironment.MapPath(MvcApplication.UsersImage) + res;
            SaveImageBase(imageUpload, path);
            return res;
        }

        protected static string SaveEnterpriseImage(HttpPostedFileBase imageUpload, string name)
        {
            var res = name + Path.GetExtension(imageUpload.FileName);
            var path = HostingEnvironment.MapPath(MvcApplication.EnterpriseImage) + res;
            SaveImageBase(imageUpload, path);
            return res;
        }

        public static void Update()
        {
            mobiMarket = new MobimarketDBEntities();
        }
    }
}