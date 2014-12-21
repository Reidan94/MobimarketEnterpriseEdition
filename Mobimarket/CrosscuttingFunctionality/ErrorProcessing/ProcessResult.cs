namespace Mobimarket.CrosscuttingFunctionality.ErrorProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    //Класс будет использоваться для оповещения пользователя 
    //при выполнении каких-то действий на сайте
    //Не смотрите на излишнюю избыточность, я его взял из туриста, если что уберем ненужное
    public class ProcessResult
    {
        public int Id { get; private set; }

        public bool Succeeded { get; private set; }

        public string Message { get; set; }

        public bool IsEmpty
        {
            get { return Succeeded == false && Message == null; }
        }

        public ProcessResult(int id, bool succeeded, string message)
        {
            Id = id;
            Succeeded = succeeded;
            Message = message;
        }
    }

    public class ProcessResults
    {
        private static readonly List<ProcessResult> Results = new List<ProcessResult>()
        {
            new ProcessResult(0, false, "Product already exists!"),
            new ProcessResult(1, false, "Wrong Email or Password!"),
            new ProcessResult(2, false, "User already exists!"),
            new ProcessResult(3, true, "Product successfully added!"),
            new ProcessResult(4, false, "Product not existing!"),
            new ProcessResult(5, true, "Product successfully edited!"),
            new ProcessResult(6, true, "Product successfully deleted!"),
            new ProcessResult(7, false, "Enterprise already exists!"),
            new ProcessResult(8, true, "Enterprise successfully added!"),
            new ProcessResult(9, true, "Enterprise successfully edited!"),
            new ProcessResult(10, true, "Enterprise successfully deactivated!"),
            new ProcessResult(11, false, "Enterprise not existing!"),
            new ProcessResult(12, false, "Not all fields filled correctly!"),
            new ProcessResult(13, true, "Enterprise successfully activated!"),
            new ProcessResult(14, true, "Contacts successfully added!"),
            new ProcessResult(15, true, "Contacts successfully deleted!"),
            new ProcessResult(16, true, "Contacts successfully edited!"),
            new ProcessResult(17, true, "Owner successfully registrated! Confirm registration via email"),
            new ProcessResult(18, false, "Owner already exists!"),
            new ProcessResult(19, false, "Owner not existing!"),
            new ProcessResult(20, false, "Error occured while sending email. Try later"),
            new ProcessResult(21, true, "Registration confirmed"),
            new ProcessResult(22, false, "Invalid email"),
            new ProcessResult(23, false, "Invalid password"),
            new ProcessResult(24, true, "Successfully logged in"),
            new ProcessResult(25, true, "Branch successfully added"),
            new ProcessResult(26, false, "Branch already exists"),
            new ProcessResult(27, false, "Invalid contact"),
            new ProcessResult(28, false, "Invalid product data")
        };

        public static ProcessResult GetById(int id)
        {
            if (id < 0 || id > Results.Count)
                throw new IndexOutOfRangeException();

            return Results[id];
        }

        public static ProcessResult ProductAlreadyExists
        {
            get { return Results[0]; }
        }

        public static ProcessResult UserAlreadyExists
        {
            get { return Results[2]; }
        }

        public static ProcessResult ProductSuccessfullyAdded
        {
            get { return Results[3]; }
        }

        public static ProcessResult ProductNotExisting
        {
            get { return Results[4]; }
        }

        public static ProcessResult ProductSuccessfullyEdited
        {
            get { return Results[5]; }
        }

        public static ProcessResult ProductSuccessfullyDeleted
        {
            get { return Results[6]; }
        }

        public static ProcessResult EnterpriseAlreadyExists
        {
            get
            {
                return Results[7];
            }
        }

        public static ProcessResult EnterpriseSuccessfullyAdded
        {
            get
            {
                return Results[8];
            }
        }

        public static ProcessResult EnterpriseSuccessfullyEdited
        {
            get
            {
                return Results[9];
            }
        }

        public static ProcessResult EnterpriseSuccessfullyDeactivated
        {
            get
            {
                return Results[10];
            }
        }

        public static ProcessResult EnterpriseNotExisting
        {
            get
            {
                return Results[11];
            }
        }

        public static ProcessResult NotAllFieldsFilledCorrectly
        {
            get
            {
                return Results[12];
            }
        }

        public static ProcessResult EnterpriseSuccessfullyActivated
        {
            get
            {
                return Results[13];
            }
        }

        public static ProcessResult ContactsSuccessfullyAdded
        {
            get
            {
                return Results[14];
            }
        }

        public static ProcessResult ContactsSuccessfullyEdited
        {
            get
            {
                return Results[15];
            }
        }

        public static ProcessResult ContactsSuccessfullyDeleted
        {
            get
            {
                return Results[16];
            }
        }

        public static ProcessResult OwnerSuccessfullyAdded
        {
            get
            {
                return Results[17];
            }
        }

        public static ProcessResult OwnerAlreadyExists
        {
            get
            {
                return Results[18];
            }
        }

        public static ProcessResult OwnerNotExisting
        {
            get
            {
                return Results[19];
            }
        }

        public static ProcessResult EmailError
        {
            get
            {
                return Results[20];
            }
        }

        public static ProcessResult RegistrationConfirmed
        {
            get
            {
                return Results[21];
            }
        }

        public static ProcessResult InvalidEmail
        {
            get
            {
                return Results[22];
            }
        }

        public static ProcessResult InvalidPassword
        {
            get
            {
                return Results[23];
            }
        }

        public static ProcessResult SuccessfullyLoggedIn
        {
            get
            {
                return Results[24];
            }
        }

        public static ProcessResult BranchSuccessfullyAdded
        {
            get
            {
                return Results[25];
            }
        }

        public static ProcessResult BranchAlreadyExists
        {
            get
            {
                return Results[26];
            }
        }

        public static ProcessResult InvalidContact
        {
            get
            {
                return Results[27];
            }
        }

        public static ProcessResult InvalidProductData
        {
            get
            {
                return Results[28];
            }
        }

    }
}