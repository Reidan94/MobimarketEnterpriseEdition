﻿using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace Mobimarket.CrosscuttingFunctionality.Senders
{
    public class ConfirmationMailSender : IMailSender
    {
        private readonly string siteMail;
        private readonly string passWord;
        private readonly string host;

        public ConfirmationMailSender()
        {
            siteMail = ConfigurationManager.AppSettings["siteMail"];
            passWord = ConfigurationManager.AppSettings["emailPassword"];
            host = ConfigurationManager.AppSettings["host"];
        }

        public bool Send(string subject, string text, string userMail)
        {
            SmtpClient smtpClient = null;
            MailMessage message = null;

            try
            {
                message = new MailMessage(new MailAddress(siteMail), new MailAddress(userMail))
                {
                    Subject = subject,
                    Body = text
                };
                smtpClient = new SmtpClient(host, Convert.ToInt32(ConfigurationManager.AppSettings["port"]))
                {
                    Credentials = new NetworkCredential(siteMail, passWord)
                };
                smtpClient.EnableSsl = true;
                smtpClient.Send(message);
                return true;
            }
            catch (SmtpException)
            {
                if (message != null)
                    message.Dispose();
                if (smtpClient != null)
                    smtpClient.Dispose();
                return false;
            }
        }
    }
}