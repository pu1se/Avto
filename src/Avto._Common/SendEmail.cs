﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Avto
{
    public static class SendEmail
    {
        public static async Task ToMyself(string subject, string body)
        {
            var emailMessage = new MailMessage
            {
                From = new MailAddress("pavel.pontus@gmail.com", $"AvtoBot"),
                To = { new MailAddress("pavel.pontus@gmail.com") },
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };
            
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Port = 587,
                Timeout = 20000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("pavel.pontus", "vbztpwqakuzicsrs")
            };

            // Ignore certificate error
            ServicePointManager.ServerCertificateValidationCallback = (_, _, _, _) => true;

            await smtp.SendMailAsync(emailMessage);
        }

        public static async Task ToSubscribedPeople(string subject, string body)
        {
            var emailMessage = new MailMessage
            {
                From = new MailAddress("pavel.pontus@gmail.com", "AvtoBot"),
                To =
                {
                    new MailAddress("pavel.pontus@gmail.com"), 
                    //new MailAddress("pontus.olya@gmail.com"), 
                    //new MailAddress("olga_pontus@tut.by"), 
                    //new MailAddress("okalobanova@gmail.com"), 
                    //new MailAddress("Info@kravmaga.by")
                },
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };
            
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Port = 587,
                Timeout = 20000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("pavel.pontus", "vbztpwqakuzicsrs")
            };

            // Ignore certificate error
            ServicePointManager.ServerCertificateValidationCallback = (_, _, _, _) => true;

            await smtp.SendMailAsync(emailMessage);
        }
    }
}
