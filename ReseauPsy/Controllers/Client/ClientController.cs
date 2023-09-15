using Business;
using ReseauPsy.ViewModel.Client;
using System;
using System.Text;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Security.Cryptography;
using NuveiClient;
using ReseauPsy.Models;
using TuesPechkin;
using System.Drawing.Printing;
using ReseauPsy.PDF.Configuration;
using ReseauPsy.Resources;

namespace ReseauPsy.Controllers.Client
{
    public class ClientController : Controller
    {

        #region Database Context

        private ReseauPsyEntities _context;

        public ClientController()
        {
            _context = new ReseauPsyEntities();
        }

        #endregion


        public ActionResult Payment(Guid? id)
        {
            if (id == null)
                return View("PaymentPageInvalid");

            var viewModel = new PaymentViewModel(_context, id.Value);

            if (!viewModel.IsGuidValid)
                return View("PaymentPageInvalid");

            ViewBag.guidUrl = id;

            return View(viewModel);
        }



        public ActionResult PayAppointment(string cardNumber, string cardExpiration, string cardCvv, string cardOwnerName, string guidUrl)
        {
            #region Server side verification

            //Card number
            Regex rgx = new Regex(@"^[0-9]{4}\s[0-9]{4}\s[0-9]{4}\s[0-9]{4}$");
            if (!rgx.IsMatch(cardNumber.Trim()))
            {
                return View();
            }

            //CardExpiration
            rgx = new Regex(@"^[0-9]{2}\/[0-9]{2}$");
            if (!rgx.IsMatch(cardExpiration.Trim()))
            {
                return View();
            }

            //Card Cvv
            rgx = new Regex(@"^[0-9]{3,4}$");
            if (!rgx.IsMatch(cardCvv))
            {
                return View();
            }

            #endregion

            #region Verification

            var guid = Guid.Parse(guidUrl);

            var clientAppointment = _context.ClientAppointments
                .Where(x => x.PaymentUrlCode == guid)
                .First();

            //On verifie si un rendez-vous existe
            if (clientAppointment == null)
                throw new Exception();

            //Verify if the appointment is already paid
            if (clientAppointment.ClientPaymentDate != null)
                throw new Exception();
            
            #endregion

            #region Appointment cost

            decimal subtotal = clientAppointment.TotalDollarAmount.Value;
            decimal tps = 0;
            decimal tvq = 0;

            GetTherapistInfo therapistInfo = new GetTherapistInfo(_context, clientAppointment.TherapistId);
            therapistInfo.GetOldTherapistInfo(clientAppointment.ClientBillGeneratedDate.Value);

            if (therapistInfo.IsTaxable)
            {
                var taxesData = new GetTaxesData(_context);
                taxesData.GetOldTaxData(clientAppointment.ClientBillGeneratedDate.Value);

                tps = subtotal * taxesData.Tps;
                tvq = subtotal * taxesData.Tvq;
            }

            decimal totalCost = subtotal + tps + tvq;

            if (clientAppointment.Clients.IsExternalClient)
            {
                GetExternalClientFeeDatas feeData = new GetExternalClientFeeDatas(_context);
                feeData.GetOldData(clientAppointment.ClientBillGeneratedDate.Value);

                decimal fees = (totalCost + feeData.FixedFee) * feeData.PercentageFeeDecimal + feeData.FixedFee;

                totalCost = totalCost + fees;
            }

            #endregion

            #region Payment type

            //By default we assume it is a payment
            var paymentType = PaymentType.PAYMENT;

            //If the current date is smaller than the appointment date, then it is a prepayment
            if (DateTime.Now < clientAppointment.StartDateTime)
                paymentType = PaymentType.PREAUTH;

            #endregion

            Nuvei nuveiPayment;


            if (paymentType == PaymentType.PAYMENT)
            {
                nuveiPayment = new NuveiPayment(
                terminalCurrency: Currencies.CAD,
                cardNumber,
                cardExpiration,
                transactionDateTime: DateTime.Now,
                Math.Round(totalCost, 2),
                cardOwnerName,
                cardCvv,
                _context);
            }
            else if (paymentType == PaymentType.PREAUTH)
            {
                nuveiPayment = new NuveiPreAuth(
                terminalCurrency: Currencies.CAD,
                cardNumber,
                cardExpiration,
                transactionDateTime: DateTime.Now,
                Math.Round(totalCost, 2),
                cardOwnerName,
                cardCvv,
                _context);
            }
            else
            {
                throw new Exception();
            }

            

            var success = nuveiPayment.UrlPayment(clientAppointment);

            bool isReceiptAvailable = false;

            if (success && (clientAppointment.AppointmentStatusId == 4 || clientAppointment.AppointmentStatusId == 5))
            {
                isReceiptAvailable = true;
            }

            if (!success)
                return Json(new { success = false });

            return Json(new { success = true, isReceiptAvailable = isReceiptAvailable });
        }


        [HttpPost]
        public ActionResult DownloadAppointmentReceipt(string guidUrl)
        {
            var guid = Guid.Parse(guidUrl);

            var clientAppointment = _context.ClientAppointments
                .Where(x => x.PaymentUrlCode == guid)
                .First();

            #region Verification if able to download

            if (clientAppointment == null)
                throw new Exception();

            //Should not be able to download the receipt if it is not payed yet
            if (clientAppointment.ClientPaymentDate == null)
                throw new Exception();

            //All but completed
            if (clientAppointment.AppointmentStatusId != 4 && clientAppointment.AppointmentStatusId != 5) 
                throw new Exception();

            #endregion

            bool isFrench = CultureInfo.CurrentCulture.Name == "fr-CA";

            string pageHtmlPathUrl = Server.MapPath("/PDF/ClientPaymentReceipt.html");
            string[] pageContentHtml = System.IO.File.ReadAllLines(pageHtmlPathUrl);


            var generateClientReceipt = new GenerateClientReceipt(clientAppointment, pageContentHtml, _context, isFrench);

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + generateClientReceipt.PdfName + " .pdf");
            Response.BinaryWrite(generateClientReceipt.PdfBuffer);
            Response.End();


            return Json(new { success = true });

        }

        public ActionResult PaymentPageInvalid()
        {
            return View();
        }


    }
}