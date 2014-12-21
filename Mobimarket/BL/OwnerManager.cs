using System.Linq;
using Mobimarket.Models;

namespace Mobimarket.BL
{
    using System;
    using System.Web;

    using CrosscuttingFunctionality.ErrorProcessing;

    using CrosscuttingFunctionality.Security;
    using CrosscuttingFunctionality.Senders;
    using System.Collections.Generic;

    public class OwnerManager : Manager
    {
        public static Owner GetOwner(Predicate<Owner> predicate)
        {
            foreach (var owner in mobiMarket.Owners.ToList())
                if (predicate(owner)) return owner;

            return null;
        }

        public static ProcessResult RegistrateOwner(HttpContextBase context, Owner owner, HttpPostedFileBase file)
        {
            if (String.IsNullOrEmpty(owner.Email)
                || string.IsNullOrEmpty(owner.LastName)
                || string.IsNullOrEmpty(owner.FirstName)
                || string.IsNullOrEmpty(owner.MiddleName)
                || string.IsNullOrEmpty(owner.Password)
                || file == null)
                return ProcessResults.NotAllFieldsFilledCorrectly;

            owner.Password = SecurityManager.GetHashString(owner.Password);
            string token = SecurityManager.GetHashString(owner.Email + owner.Password);

            var exists = GetOwner((ow) => ow.Email == owner.Email);
            if (exists != null)
                return ProcessResults.UserAlreadyExists;

            var confirmationString = context.Request.Url.GetLeftPart(UriPartial.Authority) + "/Owner/Confirm?hash=" + token;
            var body = CONFIG.CONFIRM_REGISTRATION + confirmationString;
            var confirmationMailSender = new ConfirmationMailSender();
            if (!confirmationMailSender.Send(CONFIG.REGISTRATION, body, owner.Email))
                return ProcessResults.EmailError;

            owner.PicturePath = SaveUserImage(file, owner.Email);
            owner.Active = false;
            mobiMarket.Owners.Add(owner);
            mobiMarket.SaveChanges();
            return ProcessResults.OwnerSuccessfullyAdded;
        }

        public static ProcessResult ConfirmRegistration(string hash)
        {

            foreach (var owner in mobiMarket.Owners.ToList())
            {
                if (SecurityManager.GetHashString(owner.Email + owner.Password) == hash)
                {
                    owner.Active = true;
                    mobiMarket.SaveChanges();
                    return ProcessResults.RegistrationConfirmed;
                }
            }

            return ProcessResults.OwnerNotExisting;
        }

        public static ProcessResult LogIn(string email, string password, out Owner owner)
        {
            owner = GetOwner(ow => ow.Email == email);
            if (owner == null)
                return ProcessResults.InvalidEmail;
            if (SecurityManager.GetHashString(password) != owner.Password)
                return ProcessResults.InvalidPassword;

            return ProcessResults.SuccessfullyLoggedIn;
        }

        public static List<Enterprise> GetEnterprises(int ownerId)
        {
            var enterprises = mobiMarket.Enterprises.Where(ent => ent.OwnerId == ownerId).ToList();
            return enterprises;
        }

        public static Owner DefineOwner(HttpContextBase context)
        {
            var cookieKey = context.Request.Cookies["Key"];
            if (cookieKey != null)
            {
                var emp = GetOwner(owner =>
                    SecurityManager
                    .GetHashString(owner.BirthDay
                    + owner.FirstName
                    + owner.Email) == cookieKey.Value);

                if (emp != null) return emp;
                return null;
            }

            return null;
        }
    }
}