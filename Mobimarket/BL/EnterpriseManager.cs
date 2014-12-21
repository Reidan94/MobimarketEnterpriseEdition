namespace Mobimarket.BL
{
    using System;
    using System.Linq;
    using System.Web;

    using Mobimarket.CrosscuttingFunctionality.ErrorProcessing;
    using Mobimarket.Models;

    public class EnterpriseManager : Manager
    {
        public static Enterprise GetEnterprise(Predicate<Enterprise> predicate)
        {
            return mobiMarket.Enterprises.FirstOrDefault(ent => predicate(ent));
        }

        public static ProcessResult AddEnterprise(Enterprise enterprise, HttpPostedFileBase imageUpload)
        {
            if (String.IsNullOrEmpty(enterprise.Title)
                || String.IsNullOrEmpty(enterprise.Adress)
                || String.IsNullOrEmpty(enterprise.Description))
                return ProcessResults.NotAllFieldsFilledCorrectly;

            enterprise.StartDate = DateTime.Now;
            enterprise.TimeZone = 0;
            enterprise.Balance = 0;
            enterprise.LogoPath = SaveEnterpriseImage(imageUpload, enterprise.Title);

            var exists = mobiMarket.Enterprises.Any(ent => ent.Title == enterprise.Title);
            if (exists) return ProcessResults.EnterpriseAlreadyExists;
            if (imageUpload != null)
            {
                enterprise.LogoPath = SaveEnterpriseImage(imageUpload, enterprise.Title);
            }

            enterprise.Active = true;
            mobiMarket.Enterprises.Add(enterprise);
            mobiMarket.SaveChanges();

            return ProcessResults.EnterpriseSuccessfullyAdded;
        }

        public static ProcessResult EditEnterprise(int id,
            string title,
            string adress,
            HttpPostedFileBase imageUpload)
        {
            var enterprise = GetEnterprise(ent => ent.Id == id && ent.Active);
            if (enterprise == null) return ProcessResults.EnterpriseNotExisting;
            if (!String.IsNullOrEmpty(title))
                enterprise.Title = title;

            if (!String.IsNullOrEmpty(adress))
                enterprise.Adress = adress;

            if (imageUpload != null)
            {
                enterprise.LogoPath = SaveEnterpriseImage(imageUpload, enterprise.Title);
                mobiMarket.SaveChanges();
            }

            return ProcessResults.EnterpriseSuccessfullyEdited;
        }

        public static ProcessResult DeactivateEnterprise(int id)
        {
            var enterprise = GetEnterprise(ent => ent.Id == id && ent.Active);
            if (enterprise == null) return ProcessResults.EnterpriseNotExisting;
            enterprise.Active = false;
            mobiMarket.SaveChanges();
            return ProcessResults.EnterpriseSuccessfullyDeactivated;
        }

        public static ProcessResult AddBranch(Branch branch)
        {
            var exists = mobiMarket.Branches.Any(br =>
                br.City.ToLower() == branch.City.ToLower()
                && br.Country.ToLower() == br.Country.ToLower()
                && br.Adress.ToLower() == branch.Adress.ToLower());

            if (exists) return ProcessResults.BranchAlreadyExists;
            mobiMarket.Branches.Add(branch);
            mobiMarket.SaveChanges();
            return ProcessResults.BranchSuccessfullyAdded;
        }

        public static ProcessResult AddEnterpriseContact(Contact contact)
        {
            mobiMarket.Contacts.Add(contact);
            mobiMarket.SaveChanges();
            return ProcessResults.ContactsSuccessfullyAdded;
        }

        public static ProcessResult AddProduct(Product product, HttpPostedFileBase imageUpload)
        {
            if (product.Amount < 0 ||
                    String.IsNullOrEmpty(product.Name) ||
                    imageUpload == null ||
                    product.Price < 0 ||
                    product.Amount <= 0 ||
                    product.Category < 1 ||
                    product.Category > 3
                )
                return ProcessResults.InvalidProductData;

            product.PicturePath = SaveProductImage(imageUpload, product.Name);

            mobiMarket.Products.Add(product);            
            mobiMarket.SaveChanges();
            return ProcessResults.ProductSuccessfullyAdded;
        }

        public static ProcessResult DeleteEnterpriseContact(int id)
        {
            var contact = mobiMarket.Contacts.FirstOrDefault(x => x.Id == id);
            mobiMarket.Contacts.Remove(contact);
            return ProcessResults.ContactsSuccessfullyDeleted;
        }

        public static ProcessResult ActivateEnterprise(int id)
        {
            var enterprise = GetEnterprise(ent => ent.Id == id && ent.Active);
            if (enterprise == null) return ProcessResults.EnterpriseNotExisting;
            enterprise.Active = true;
            mobiMarket.SaveChanges();
            return ProcessResults.EnterpriseSuccessfullyActivated;
        }

        public static Enterprise GetEnterprise(int id)
        {
            return mobiMarket.Enterprises.FirstOrDefault(x => x.Id == id);
        }
    }
}