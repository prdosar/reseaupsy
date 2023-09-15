using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HelpersLibrary
{
    public class EmailHelper
    {
        /// <summary>
        /// Procédure d'envoie de courriel 
        /// </summary>
        /// <param name="subject">Le subjet du courriel</param>
        /// <param name="content">Le contenu du courriel</param>
        /// <param name="emailTo">L'addresse courriel du destinataire</param>
        /// <param name="emailReplyTo">L'addresse courriel pour l'option "répondre", laisser vide si utilise pas</param>
        /// <param name="emailFrom">Les infos de l'expéditeur du courriel</param>
        public static void Send(string subject, string content, string emailTo, string emailReplyTo, string emailFrom = null)
        {
            if (bool.Parse(ConfigurationManager.AppSettings["USETLS12"].ToString().ToLower()))
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            }

            MailMessage netMail = new MailMessage();
            SmtpClient mailClient = new SmtpClient();

            string emailLogin = ConfigurationManager.AppSettings["EmailLogin"];
            string emailPassword = ConfigurationManager.AppSettings["EmailPassword"];

            // Si le courriel from pas fournie, le récupère du app.config
            if (string.IsNullOrEmpty(emailFrom))
            {
                // Récupère du app.config
                netMail.From = new MailAddress(string.Format("{0} <{1}>", ConfigurationManager.AppSettings["EmailFromName"], emailLogin));
            }
            else
            {
                // Utilise celui passé en paramètre
                netMail.From = new MailAddress(emailFrom);
            }

            netMail.IsBodyHtml = true;

            // Configuration du système de courriel
            mailClient.Host = ConfigurationManager.AppSettings["EmailSmtpHost"];
            mailClient.Port = int.Parse(ConfigurationManager.AppSettings["EmailHostPort"]);
            mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            mailClient.UseDefaultCredentials = false;
            mailClient.Credentials = new NetworkCredential(emailLogin, emailPassword);
            mailClient.EnableSsl = bool.Parse(ConfigurationManager.AppSettings["UseSSL"].ToString().ToLower());

            // Détermine si utilise le courriel passé en paramètre ou celui live
            if (ConfigurationManager.AppSettings["EmailDebugMode"] == "1")
            {
                // Récupère le courriel pour les teste
                netMail.To.Add(ConfigurationManager.AppSettings["EmailDebugTo"]);
                content += "<Br /> Email to --> " + emailTo;
            }
            else
            {
                // Utilise celui passé en paramètre
                netMail.To.Add(emailTo);
            }

            // Détermine si dois utilisé l'option de replyto
            if (!string.IsNullOrEmpty(emailReplyTo))
            {
                // Pas vide, donc insère l'adresse pour l'option "répondre"
                netMail.ReplyToList.Add(emailReplyTo);
            }

            //Outlook ne format ce caractère donc ceci est un patch car il n'y a pas d'autre solution
            content = content.Replace("&apos;", "'");

            // Construit le courriel
            netMail.Subject = subject;
            netMail.Body = content;

            // Envoie le courriel
            mailClient.Send(netMail);

            // Détruit les instence de l'objet
            netMail.Dispose();
            mailClient.Dispose();
        }
    }
}
