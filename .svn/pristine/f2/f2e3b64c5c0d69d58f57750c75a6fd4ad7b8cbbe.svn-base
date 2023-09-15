using Business;
using HelpersLibrary;
using ReseauPsy.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ReseauPsy.Models
{
    public class ResendClientReceipt
    {
        public void ResendReceiptByEmail(string emailClient, int clientAppointmentId, ReseauPsyEntities context)
        {
            var guidUrl = context.ClientAppointments
                .Where(x => x.Id == clientAppointmentId)
                .Select(x => x.PaymentUrlCode)
                .Single();

            var requestContext = HttpContext.Current.Request.RequestContext;
            string receiptLink = new UrlHelper(requestContext).Action(
                "Payment",
                "Client",
                null,
                HttpContext.Current.Request.Url.Scheme);

            receiptLink += $"/{guidUrl}";

            var appointment = context.ClientAppointments
                .Where(x => x.Id == clientAppointmentId)
                .Single();

            var resourceManager = new ResourceManager(typeof(Email));
            var culture = new CultureInfo(appointment.Clients.LanguageId == 1 ? "fr-CA" : "en-CA");

            var emailSubject = resourceManager.GetString("ResendReceipt_Subject", culture);
            var emailContent = string.Format(resourceManager.GetString("ResendReceipt_Content", culture),
                receiptLink);

            EmailHelper.Send(emailSubject, emailContent, emailClient, "");
        }
    }
}