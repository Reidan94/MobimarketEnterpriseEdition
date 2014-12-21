using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mobimarket.CrosscuttingFunctionality.ErrorProcessing;
using Mobimarket.Models;

namespace Mobimarket.BL
{
    public class ProductManager : Manager
    {

        public static Product GetProduct(Predicate<Product> isEqual)
        {
            return mobiMarket.Products.FirstOrDefault(product => isEqual(product));
        }

        public static ProcessResult AddProduct(int enterpriseId, Product product, HttpPostedFileBase imageUpload)
        {
            var enterprise = EnterpriseManager.GetEnterprise(ent => ent.Id == enterpriseId);
            if (enterprise == null) return ProcessResults.EnterpriseNotExisting;
            var exists = mobiMarket.Products.Any(pr => pr.Name == product.Name);
            if (exists)
                return ProcessResults.ProductAlreadyExists;

            product.PicturePath = SaveProductImage(imageUpload, product.Name);
            mobiMarket.Products.Add(product);
            mobiMarket.SaveChangesAsync();
            return ProcessResults.ProductSuccessfullyAdded;
        }

        public static ProcessResult EditProduct(int productId, Product newProduct, HttpPostedFileBase imageUpload)
        {
            var product = GetProduct(x => x.Id == productId);
            if (product == null)
                return ProcessResults.ProductNotExisting;

            product.Name = newProduct.Name;
            product.Price = newProduct.Price;
            product.PicturePath = imageUpload != null ? SaveProductImage(imageUpload, product.Name) : String.Empty;

            mobiMarket.SaveChanges();
            return ProcessResults.ProductSuccessfullyEdited;
        }

        public static ProcessResult DeleteProduct(int productId)
        {
            var product = GetProduct(pr => pr.Id == productId);
            if (product == null)
                return ProcessResults.ProductNotExisting;

            mobiMarket.Products.Remove(product);
            mobiMarket.SaveChanges();
            return ProcessResults.ProductSuccessfullyDeleted;
        }

        public static List<Product> GetProducts(int enterpriseId)
        {
            return mobiMarket
                .Products
                .Where(product => product.EnterpriseId == enterpriseId)
                .OrderBy(product =>
                mobiMarket
                .Orders
                .Count(order => order
                    .OrderItems
                    .Any(orderItem => orderItem
                        .ProductId == product.Id))).ToList();
        }
    }
}