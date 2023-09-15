using Business;
using Library.Helper;
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
    public class GenerateClientReceipt
    {
        public byte[] PdfBuffer { get; set; }
        public string PdfName { get; set; }

        public GenerateClientReceipt(ClientAppointments clientAppointment, string[] pageContentHtml, 
            ReseauPsyEntities context, bool isFrench)
        {
            // Convertie le array en string
            string pdfContent = string.Empty;
            int pageContentHtmlLength = pageContentHtml.Length;
            for (int cpt = 0; cpt < pageContentHtmlLength; cpt++)
            {
                pdfContent += pageContentHtml[cpt].Trim();
            }

            //Recupere les info temporelle du therapeute
            var getTherapistInfo = new GetTherapistInfo(context, clientAppointment.Therapists.Id);
            getTherapistInfo.GetOldTherapistInfo(clientAppointment.ClientBillGeneratedDate.Value);

            #region Therapist title

            string therapistTitle = "";

            if (!string.IsNullOrWhiteSpace(getTherapistInfo.OpqNumber))
            {
                var accreditation = context.Accreditations
                    .Where(x => x.Id == getTherapistInfo.AccreditationId)
                    .First();

                var accreditationName = isFrench ? accreditation.Accreditation1 : accreditation.AccreditationEN;
                therapistTitle = accreditationName + " (OPQ : " + getTherapistInfo.OpqNumber + ")";
            }
            else if (getTherapistInfo.Associations.Count == 0)
            {
                therapistTitle = "";
            }
            else
            {
                therapistTitle += Global.AssociationOrOrder + "<br/>";

                foreach (var title in getTherapistInfo.Associations)
                {
                    therapistTitle += title.AssociationName.Decode() + " (" + title.AssociationNumber.Decode() + ")<br />";
                }
            }

            #endregion


            var getAppointmentCost = new GetAppointmentCost(context, clientAppointment.Id);


            var taxesData = new GetTaxesData(context);
            taxesData.GetOldTaxData(clientAppointment.ClientBillGeneratedDate.Value);

            var rspyInfo = new GetReseauPsyInfo(context);
            rspyInfo.GetOldInfo(clientAppointment.ClientBillGeneratedDate.Value);

            var clientInfos = new GetClientInfos(context, clientAppointment.ClientId);
            clientInfos.GetOldClientInfo(clientAppointment.StartDateTime);


            string appointmentType = string.Format(Resource.ClientReceiptPdf_Name_AppointmentType,
                isFrench ? clientAppointment.NatureActs.NatureAct1 : clientAppointment.NatureActs.NatureActEN,
                isFrench ? clientAppointment.ProfessionalTitles.Secteur : clientAppointment.ProfessionalTitles.SecteurEN);

            string consultationTypeName = isFrench ? clientAppointment.ConsultationTypes.ConsultationType1 : clientAppointment.ConsultationTypes.ConsultationTypeEN;




            pdfContent = pdfContent
                .Replace("__AppointmentDateTitle__", Resource.ClientReceiptPdf_Label_AppointmentDate)
                .Replace("__TherapistFullNameTitle__", Resource.ClientReceiptPdf_Label_TherapistName)
                .Replace("__RspyAdress__", rspyInfo.Adress)
                .Replace("__RspyCity__", rspyInfo.City)
                .Replace("__RspyRegionAbbr__", rspyInfo.RegionAbbreviation)
                .Replace("__RspyPostal__", rspyInfo.PostalCode)
                .Replace("__RspyPhoneNumber__", rspyInfo.PhoneNumber)
                .Replace("__RspyEmail__", rspyInfo.Email)
                .Replace("__RspyTpsNumber__", rspyInfo.TpsNumber)
                .Replace("__RspyTvqNumber__", rspyInfo.TvqNumber)
                .Replace("__ReceiptForInsuranceTitle__", Resource.ClientReceiptPdf_Label_ReciptForInsurance)
                .Replace("__AppointmentDate__", clientAppointment.StartDateTime.ToString("dd-MM-yyyy"))
                .Replace("__AppointmentDuration__", (clientAppointment.EndDateTime - clientAppointment.StartDateTime).TotalMinutes + "Min")
                .Replace("__TherapistFullName__", getTherapistInfo.FirstName + " " + getTherapistInfo.LastName)
                .Replace("__TherapistTitle__", therapistTitle)
                .Replace("__OfficeAdress__", getTherapistInfo.Adress)
                .Replace("__OfficeAdressCity__", getTherapistInfo.City)
                .Replace("__OfficeAdressPostalCode__", getTherapistInfo.PostalCode)
                .Replace("__ConsultationType__", consultationTypeName)
                .Replace("__ConsultationTypeLabel__", Global.ConsultationType)

                


                .Replace("__TherapistNumber__", getTherapistInfo.PhoneNumber)
                .Replace("__ClientFullName__", clientInfos.FirstName.Decode() + " " + clientInfos.LastName.Decode())
                .Replace("__SubtotalTitle__", Resource.TherapistBillPdf_Label_Subtotal)
                .Replace("__TotalTitle__", Resource.TherapistBillPdf_Label_Total)
                .Replace("__TpsPercent__", taxesData.Tps.ToString("P"))
                .Replace("__TvqPercent__", taxesData.Tvq.ToString("P3"))
                .Replace("__SubTotal__", getAppointmentCost.SubTotal.ToString("C"))
                .Replace("__TPS__", getAppointmentCost.Tps.ToString("C"))
                .Replace("__TVQ__", getAppointmentCost.Tvq.ToString("C"))
                .Replace("__Total__", getAppointmentCost.Total.ToString("C"))
                .Replace("__ReceiptNumber__", "#" + clientAppointment.ClientPaymentReceiptNumber.Value.ToString("000000"));
                





            if (getAppointmentCost.Tps == 0 && getAppointmentCost.Tvq == 0)
            {
                //On enleve tout ce qui a un lien avec les taxes
                pdfContent = pdfContent.Replace("client-receipt-tax-block", "client-receipt-tax-block d-none");
            }

            if (clientAppointment.AppointmentStatusId == 4)
            {
                //Si c'est annulé avec charge 
                pdfContent = pdfContent.Replace("__ReceiptTitle__", Resource.ClientReceiptPdf_Label_AdministrativeCharges);

            }
            else
            {
                pdfContent = pdfContent.Replace("__ReceiptTitle__", appointmentType);
            }

            if (true)
            {

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

            this.PdfName = $"{clientInfos.FirstName} {clientInfos.LastName} - {clientAppointment.StartDateTime.ToString("yyyy-MM-dd")}";
            this.PdfName = this.PdfName.Decode();

            GlobalSettings globalConfig = new GlobalSettings
            {
                ProduceOutline = true,
                DocumentTitle = this.PdfName + ".pdf",
                PaperSize = PaperKind.Letter,
                Orientation = GlobalSettings.PaperOrientation.Landscape,
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