using Business;
using Library.Helper;
using ReseauPsy.Models.Therapist;
using ReseauPsy.PDF.Configuration;
using ReseauPsy.Resources;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Web;
using TuesPechkin;

namespace ReseauPsy.Models
{
    public class GenerateTherapistBillPdf
    {
        public byte[] PdfBuffer { get; set; }
        public string PdfName { get; set; }

        public GenerateTherapistBillPdf(string[] pageContentHtml, List<ClientAppointments> clientAppointments,
            ReseauPsyEntities context, TherapistBill therapistBill)
        {
            // Convertie le array en string
            string pdfContent = string.Empty;
            int pageContentHtmlLength = pageContentHtml.Length;
            for (int cpt = 0; cpt < pageContentHtmlLength; cpt++)
            {
                pdfContent += pageContentHtml[cpt].Trim();
            }

            string appointmentMarkupPDF = "";

            //Get the pay information of the therapist
            var therapistPayInfo = context.TherapistPayInformations
                .Where(x => x.TherapistId == therapistBill.Therapists.Id)
                .ToList();

            foreach (var appointment in clientAppointments)
            {
                var clientInfos = new GetClientInfos(context, appointment.ClientId);
                clientInfos.GetOldClientInfo(appointment.ClientBillGeneratedDate.Value);

                var getClaimableAmountForAppointment = new GetClaimableAmountForAppointment(context);
                getClaimableAmountForAppointment.CalculateAppointmentPayment(appointment.Id);

                //Add the markup for the pdf
                appointmentMarkupPDF +=
                    "<tr>" +
                    $"<td>{appointment.StartDateTime.ToString("yyyy-MM-dd")} - {clientInfos.FirstName} {clientInfos.LastName}</td>" +
                    $"<td>{appointment.NbSession:0.#}</td>" +
                    $"<td>{getClaimableAmountForAppointment.HourlyWage}</td>" +
                    $"<td>{(getClaimableAmountForAppointment.ClaimableAmount):0.00}</td>" +
                    "</tr>";
            }

            var getTherapistInfo = new GetTherapistInfo(context, therapistBill.Therapists.Id);
            getTherapistInfo.GetOldTherapistInfo(therapistBill.Date);

            var taxesData = new GetTaxesData(context);
            taxesData.GetOldTaxData(therapistBill.Date);

            var rspyInfo = new GetReseauPsyInfo(context);
            rspyInfo.GetOldInfo(therapistBill.Date);

            string tpsNumber = getTherapistInfo.TpsNumber;
            string tvqNumber = getTherapistInfo.TvqNumber;

            if (therapistBill.TpsAmount > 0 && string.IsNullOrWhiteSpace(tpsNumber))
            {
                var oldTaxInfo = context.TherapistInfo
                    .Where(x => x.TherapistId == therapistBill.Therapists.Id && x.ModificationDate < therapistBill.Date
                        && x.TpsNumber != null && x.TvqNumber != null)
                    .OrderByDescending(x => x.ModificationDate)
                    .First();

                tpsNumber = oldTaxInfo.TpsNumber;
                tvqNumber = oldTaxInfo.TvqNumber;
            }

            pdfContent = pdfContent
                .Replace("__BillNumberTitle__", Resource.TherapistBillPdf_Label_BillNumber)
                .Replace("__BillTitle__", Resource.TherapistBillPdf_Label_BillTitle)
                .Replace("__DateTitle__", Resource.TherapistBillPdf_Label_DateTitle)
                .Replace("__BilledAtTitle__", Resource.TherapistBillPdf_Label_BilledAt)
                .Replace("__DescriptionTitle__", Resource.TherapistBillPdf_Th_Description)
                .Replace("__QuantityTitle__", Resource.TherapistBillPdf_Th_Quantity)
                .Replace("__HourlyWageTitle__", Resource.TherapistBillPdf_Th_HourlyWage)
                .Replace("__AmountTitle__", Resource.TherapistBillPdf_Th_Amount)
                .Replace("__SubtotalTitle__", Resource.TherapistBillPdf_Label_Subtotal)
                .Replace("__TotalTitle__", Resource.TherapistBillPdf_Label_Total)
                .Replace("<!-- Insert appointments -->", appointmentMarkupPDF)
                .Replace("__RspyAdress__", rspyInfo.Adress)
                .Replace("__RspyCity__", rspyInfo.City)
                .Replace("__RspyRegionAbbr__", rspyInfo.RegionAbbreviation)
                .Replace("__RspyPostalCode__", rspyInfo.PostalCode)
                .Replace("__RspyEmail__", rspyInfo.Email)
                .Replace("__TherapistName__", getTherapistInfo.FirstName + " " + getTherapistInfo.LastName)
                .Replace("__TherapistAdress__", getTherapistInfo.Adress)
                .Replace("__TherapistCity__", getTherapistInfo.City)
                .Replace("__TherapistEmail__", getTherapistInfo.Email)
                .Replace("__TherapistPhoneNumber__", getTherapistInfo.PhoneNumber)
                .Replace("__BillDate__", therapistBill.Date.ToString("yyyy-MM-dd"))
                .Replace("__BillNumber__", therapistBill.BillNumber.ToString())
                .Replace("__TherapistTpsNumber__", tpsNumber)
                .Replace("__TherapistTvqNumber__", tvqNumber)
                .Replace("__SubTotal__", therapistBill.SubTotal.ToString())
                .Replace("__TpsPercent__", taxesData.Tps.ToString("P"))
                .Replace("__TvqPercent__", taxesData.Tvq.ToString("P3"))
                .Replace("__TPS__", therapistBill.TpsAmount.ToString())
                .Replace("__TVQ__", therapistBill.TvqAmount.ToString())
                .Replace("__Total__", (therapistBill.SubTotal + therapistBill.TpsAmount + therapistBill.TvqAmount).ToString());


            if (therapistBill.TpsAmount == 0 && therapistBill.TvqAmount == 0)
            {
                //Il faut cacher tout ce qui est en lien avec les taxes
                pdfContent = pdfContent.Replace("therapist-bill-taxes", "therapist-bill-taxes d-none");
            }


            // Initialise le PDF
            ObjectSettings documentConfig = new ObjectSettings
            {
                WebSettings = new WebSettings
                {
                    EnableJavascript = true,
                    PrintBackground = true,
                    PrintMediaType = true,
                    DefaultEncoding = "utf-8"
                },
                LoadSettings = new LoadSettings
                {
                    BlockLocalFileAccess = false,
                },
                HtmlText = pdfContent
            };

            this.PdfName = $"{getTherapistInfo.FirstName} {getTherapistInfo.LastName} RSPYF-{therapistBill.BillNumber}";
            this.PdfName = this.PdfName.Decode();
            GlobalSettings globalConfig = new GlobalSettings
            {
                ProduceOutline = true,
                DocumentTitle = this.PdfName + ".pdf",
                PaperSize = PaperKind.Letter,
                DPI = 72,
                UseCompression = true,
                OutputFormat = GlobalSettings.DocumentOutputFormat.PDF,
                Margins = new MarginSettings
                {
                    Unit = TuesPechkin.Unit.Centimeters,
                    Top = 1,
                    Right = 1,
                    Bottom = 1,
                    Left = 1
                }
            };

            HtmlToPdfDocument docPdf = new HtmlToPdfDocument
            {
                Objects = { documentConfig },
                GlobalSettings = globalConfig
            };

            // Download le pdf
            IConverter converter = TuesPechkinConverter.Converter;
            this.PdfBuffer = converter.Convert(docPdf);
        }
    }
}