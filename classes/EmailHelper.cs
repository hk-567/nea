using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;

namespace NEA
{
    public class EmailHelper
    {
            private static string emailFrom = "*****@gmail.com";
        private static string emailPassword = "***************";

        public static void SendActivationEmail(string email, string activationCode)
            {
                MailMessage mail = new MailMessage();
                SmtpClient client = new SmtpClient();

                // Set the email properties
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(email);
                mail.Subject = "Account Activation";
                mail.Body = "Please click on the following link to activate your account: http://localhost:port/activate.aspx?code=" + activationCode;

                // Set the SMTP client properties
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(emailFrom, emailPassword);

                // Send the email
                client.Send(mail);
            }

        }
    }
