using Mobimarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Mobimarket.CrosscuttingFunctionality.Security
{
    public static class Validator
    {
        public static bool ValidatePhoneNumber(string phone)
        {
            foreach (var digit in phone)
            {
                if (!Char.IsNumber(digit))
                    return false;
            }
            return true;
        }

        public static bool ValidateContact(string contact, ContactType type)
        {
            if (type == ContactType.Facebook) return ValidateFacebookRef(contact);
            if (type == ContactType.LinkedIn) return ValidateLinkedInRef(contact);
            return ValidatePhoneNumber(contact);
        }
        public static bool IsValidUri(string uriString)
        {
            Uri uri;
            if (!uriString.Contains("://")) uriString = "http://" + uriString;
            if (Uri.TryCreate(uriString, UriKind.RelativeOrAbsolute, out uri))
            {
                try
                {
                    if (Dns.GetHostAddresses(uri.DnsSafeHost).Length > 0)
                    {
                        return true;
                    }
                }
                catch 
                {
                    return false;
                }
            }
            return false;
        }

        public static bool ValidateFacebookRef(string facebookRef)
        {
            if (!IsValidUri(facebookRef)) return false;
            if (!facebookRef.ToLower().Contains("facebook")) return false;

            return true;
        }

        public static bool ValidateLinkedInRef(string linkedInRef)
        {
            if (!IsValidUri(linkedInRef)) return false;
            if (!linkedInRef.Contains("linkedin")) return false;

            return true;
        }
    }
}