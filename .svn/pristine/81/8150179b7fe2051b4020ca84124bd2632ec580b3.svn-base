using Business;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using ReseauPsy.Models;
using ReseauPsy.Models.Admin;
using ReseauPsy.ViewModel.Admin;
using System;
using Library.Helper;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Collections.Generic;
using System.Resources;
using ReseauPsy.Resources;
using System.Globalization;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using OfficeOpenXml;
using System.IO;
using ReseauPsy.Helper;
using HelpersLibrary;
using TuesPechkin;
using System.Drawing.Printing;
using ReseauPsy.PDF.Configuration;
using System.Threading;

namespace ReseauPsy.Controllers.Admin
{
    [Authorize(Roles = RoleName.AdminFullAccess)]
    public class AdminController : Controller
    {
        #region Context

        private ReseauPsyEntities _context;
        public AdminController()
        {
            _context = new ReseauPsyEntities();
        }

        #endregion

        private int _nbResultPerPage
        {
            get
            {
                return WebSiteProperties.NbResultPerPage;
            }
        }

        #region List admin

        public ActionResult AdminList()
        {
            var viewModel = new ListAdminViewModel(_context);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult TableAdminList(bool isActive, int pageNumber)
        {
            var admins = _context.GetListAdmin(
                !isActive,
                pageNumber,
                _nbResultPerPage);

            //var nbPage = _context.GetListAdminCount(!isActive).First();

            var nbPage = Math.Ceiling(
                Convert.ToDecimal(_context.GetListAdminCount(!isActive).First()) / Convert.ToDecimal(_nbResultPerPage));

            return Json(new
            {
                admins = admins,
                nbPage = nbPage
            });
        }

        [HttpPost]
        public ActionResult DeleteAdmin(string adminId)
        {
            try
            {
                var admin = new LeadManagement.Admin { Id = int.Parse(adminId) };
                admin.DeleteAdmin(_context);

                return Json(new
                {
                    msg = "Successfully deleted " + adminId
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult RestoreAdmin(int adminId)
        {
            var admin = new LeadManagement.Admin { Id = adminId };
            admin.RestoreAdmin(_context);
            return Json(new
            {
                msg = "Successfully restored " + adminId
            });
        }

        #endregion

        #region Detail admin

        public ActionResult DetailAdmin(int? adminId)
        {
            DetailsAdminViewModel viewModel;
            if (adminId != null)
            {
                viewModel = new DetailsAdminViewModel(_context, adminId.Value);
            }
            else
            {
                viewModel = new DetailsAdminViewModel();
            }


            return View("DetailsAdmin", viewModel);
        }


        public ActionResult CreateAdmin(string name, string email, string password)
        {
            var admin = new LeadManagement.Admin
            {
                Name = name,
                Email = email,
                Password = password
            };


            if (!VerifyIdentityEmail.VerifyEmailAvailabillity(_context, email))
            {
                //Le email existe déjà
                ViewBag.ErrorMessage = Resources.Global.Email_AlreadyUsed;
                return Json(new { success = false, error = "emailUsed" });
            }

            if (!admin.CreateAdmin(_context))
            {
                throw new Exception();
            }

            TempData["Message"] = Resource.AdminDetail_CreateSuccess;

            return Json(new { result = "Redirect", url = Url.Action("AdminList", "admin") });
        }

        public ActionResult EditAdmin(int adminId, string name, string email, string password)
        {

            var admin = new LeadManagement.Admin(_context.Admins
                .Single(x => x.Id == adminId));

            if (admin.Email != email)
            {
                //Le email a changé
                admin.Email = email;
                if (!VerifyIdentityEmail.VerifyEmailAvailabillity(_context, admin.Email))
                {
                    //The entered email is already in used
                    return Json(new { success = false, error = "emailUsed" });
                }
            }
            if (!admin.ModifyAdmin(_context, name, email, password))
            {
                //La modification à échoué
                throw new Exception();
            }

            return Json(new { success = true });
        }

        #endregion

        #region List therapist

        public ActionResult TherapistList()
        {
            var viewModel = new TherapistListViewModel(_context);
            return View("TherapistList", viewModel);
        }


        [HttpPost]
        public ActionResult TableTherapistList(TherapistListFilters filters)
        {
            var monday = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            var sunday = monday.AddDays(7);
            List<GetTherapistListForAdmin_Result> therapists = _context.GetTherapistListForAdmin(
                filters.IsDeleted,
                monday,
                sunday,
                filters.IsAvailable,
                filters.GenderId,
                filters.ClientTypeId,
                filters.ConsultationTypeId,
                filters.ClientAgeRangeId,
                filters.RegionId,
                filters.ProfessionalTitleId,
                filters.LanguageId,
                filters.OfferedServices,
                filters.WeekAvailabilities,
                filters.PageNumber,
                _nbResultPerPage)
                .ToList();

            var nbResult = _context.GetTherapistListForAdminCount(
                filters.IsDeleted,
                monday,
                sunday,
                filters.IsAvailable,
                filters.GenderId,
                filters.ClientTypeId,
                filters.ConsultationTypeId,
                filters.ClientAgeRangeId,
                filters.RegionId,
                filters.ProfessionalTitleId,
                filters.LanguageId,
                filters.OfferedServices,
                filters.WeekAvailabilities).First();

            var nbPage =
                Convert.ToInt32(
                    Math.Ceiling(
                        Convert.ToDecimal(nbResult)
                        /
                        Convert.ToDecimal(WebSiteProperties.NbResultPerPage)
                    )
                );

            return Json(new
            {
                therapists = therapists,
                nbPage = nbPage
            });
        }


        [HttpPost]
        public ActionResult DeleteTherapist(int therapistId)
        {
            var therapist = _context.Therapists
                .Where(x => x.Id == therapistId)
                .Single();

            therapist.IsDeleted = true;

            try
            {
                _context.SaveChanges();
                return Json(new { msg = "Success" });
            }
            catch (Exception)
            {

                return Json(new { msg = "Failed" });
            }
        }

        [HttpPost]
        public ActionResult RestoreTherapist(int therapistId)
        {
            var therapist = _context.Therapists
                .Where(x => x.Id == therapistId)
                .Single();

            therapist.IsDeleted = false;

            try
            {
                _context.SaveChanges();
                return Json(new { msg = "Success" });
            }
            catch (Exception)
            {

                return Json(new { msg = "Failed" });
            }
        }

        #endregion

        #region Detail therapist

        public ActionResult DetailTherapist(int? therapistId)
        {
            DetailsTherapistViewModel viewModel;

            if (therapistId != null)
            {
                var therapist = _context.Therapists.SingleOrDefault(x => x.Id == therapistId);

                if (therapist == null)
                    return RedirectToAction("TherapistList");

                viewModel = new DetailsTherapistViewModel(_context, therapist);
            }
            else
            {
                viewModel = new DetailsTherapistViewModel(_context);
            }

            return View("DetailsTherapist", viewModel);
        }

        public ActionResult CreateTherapist(DetailTherapistDatas datas)
        {

            #region Verify the datas

            if (datas.FirstName.Trim() == "" ||
                datas.LastName.Trim() == "" ||
                datas.Email.Trim() == "" ||
                datas.HiringDate == null ||
                datas.Wages.Count < 1)
            {
                throw new Exception();
            }

            #endregion

            if (!VerifyIdentityEmail.VerifyEmailAvailabillity(_context, datas.Email))
            {
                //Le email est deja en utilisation, on renvoie le form
                return Json(new { success = false, error = "emailUsed" });
            }

            var createTherapist = new CreateTherapist(datas, _context);
            if (!createTherapist.Succeeded)
            {
                //La création du thérapeute a échoué, on retourne le form
                throw new Exception();
            }


            TempData["Message"] = Resource.DetailTherapist_SuccedeedCreated;

            return Json(new { result = "Redirect", url = Url.Action("TherapistList", "admin") });

        }

        public ActionResult EditTherapist(DetailTherapistDatas datas)
        {
            #region Verify the datas

            if (datas.FirstName.Trim() == "" ||
                datas.LastName.Trim() == "" ||
                datas.Email.Trim() == "" ||
                datas.HiringDate == null ||
                datas.Wages.Count < 1)
            {
                throw new Exception();
            }

            #endregion

            #region On verifie le courriel

            var therapistInfo = new GetTherapistInfo(_context, datas.TherapistId.Value);
            therapistInfo.GetMostRecentTherapistInfo();

            if (therapistInfo.Email != datas.Email)
            {
                //On change le email. On doit vérifier si le nouveau courriel n'est pas en utilisation
                if (!VerifyIdentityEmail.VerifyEmailAvailabillity(_context, datas.Email))
                {
                    //Le courriel est deja en utilisation, on renvoie le form
                    return Json(new { success = false, error = "emailUsed" });
                }
            }

            #endregion

            var editTherapist = new EditTherapist(datas, _context, datas.TherapistId.Value);
            if (!editTherapist.Succeeded)
            {
                //La modification du thérapeute a échoué, on renvoie le form
                throw new Exception();
            }

            TempData["Message"] = Resource.DetailTherapist_SuccedeedModified;

            return Json(new
            {
                result = "Redirect",
                url = Url.Action("TherapistList", "admin")
            });

        }

        #endregion

        #region Appointment Request
        public ActionResult AppointmentRequest()
        {
            var viewModel = new AppointmentRequestViewModel(_context);
            return View(viewModel);
        }


        public ActionResult AppointmentClientTable(bool isRequestToDo, DateTime? startDate, DateTime? endDate,
            int pageNumber, bool isFrench)
        {

            if (endDate != null)
            {
                //On veut inclure la derniere journée
                endDate = endDate.Value.AddDays(1).AddSeconds(-1);
            }

            var clients = _context.GetListClientForClientRequestPage(
                isRequestToDo,
                isFrench,
                startDate,
                endDate,
                pageNumber,
                isRequestToDo ? 999_999 : _nbResultPerPage)
                .Select(x => new ClientRequestTable(x, isFrench))
                .ToList();

            var count = _context.GetListClientForClientRequestPageCount(
                isRequestToDo,
                isFrench,
                startDate,
                endDate).First();

            var nbPage = isRequestToDo ? 1 :
                Convert.ToInt32(
                    Math.Ceiling(
                        Convert.ToDecimal(count)
                        /
                        Convert.ToDecimal(WebSiteProperties.NbResultPerPage)
                    )
                );

            return Json(new
            {
                clients = clients,
                nbPage = nbPage
            });
        }



        public ActionResult SetAssignationTherapistSelect(int clientRegionId, string clientConsultationTypeIds)
        {
            var monday = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            var sunday = monday.AddDays(7);
            var therapists = _context.GetListTherapistForClientRequestPage(
                monday,
                sunday,
                clientRegionId,
                clientConsultationTypeIds)
                .ToList();

            return Json(new { therapists = therapists });
        }

        public ActionResult DeleteClientRequest(int clientId, int deleteReasonId, string notes)
        {
            var admin = new LeadManagement.Admin();

            var success = admin.DeleteClientRequest(_context, clientId, deleteReasonId, notes);

            if (success)
            {
                return Json(new
                {
                    success = "success"
                });
            }
            else
            {
                throw new Exception();
            }
        }


        public ActionResult SendClientRequestToTherapistOrAssociation(bool isSendTherapist, int therapistOrAssociationId, int clientId, string notes)
        {
            var admin = new LeadManagement.Admin();
            var success = admin.AssignClientRequestToTherapistOrAssociation(
                    _context,
                    isSendTherapist,
                    clientId,
                    therapistOrAssociationId,
                    notes);

            if (success)
            {
                return Json(new
                {
                    success = "success"
                });
            }
            else
            {
                throw new Exception();
            }
        }


        public ActionResult RestoreClient(int clientId)
        {
            var admin = new LeadManagement.Admin();
            var success = admin.RestoreClientRequest(_context, clientId);
            if (success)
            {
                return Json(new
                {
                    success = "success"
                });
            }
            else
            {
                throw new Exception();
            }
        }


        public ActionResult RetrieveClientRequest(int clientId)
        {
            var admin = new LeadManagement.Admin();
            var success = admin.RetrieveClientRequest(_context, clientId);
            if (success)
            {
                return Json(new
                {
                    success = "success"
                });
            }
            else
            {
                throw new Exception();
            }
        }

        #endregion

        #region External Association

        public ActionResult ExternalAssociation()
        {
            var viewModel = new ExternnalAssociationListViewModel(_context);
            return View(viewModel);
        }

        public ActionResult ChangeClientStatus(int externalAssociationClientSentId, int statusId)
        {
            var externalAssociationClientSent = _context.ExternalAssociationClientsSents
                .Where(x => x.Id == externalAssociationClientSentId)
                .Single();

            externalAssociationClientSent.ExternalAssociationClientSentStatusId = statusId;
            externalAssociationClientSent.StatusChangedDate = DateTime.Now;

            _context.SaveChanges();

            return Json(new { success = "success" });
        }
        public ActionResult ApplyExternalAssociationClientFilter(bool isFrench, DateTime? startDate, DateTime? endDate, int? statusId, int? associationId, int pageNumber)
        {
            if (endDate != null)
                endDate = endDate.Value.AddDays(1).AddSeconds(-1);

            var clients = _context.GetClientSentToExternalAssociation(
                isFrench,
                startDate,
                endDate,
                statusId,
                associationId,
                pageNumber,
                _nbResultPerPage)
                .ToList();

            var count = _context.GetClientSentToExternalAssociationCount(
                startDate,
                endDate,
                statusId,
                associationId).ToList();

            var countNotDefined = count[0].NotDefinedCount;
            var countTookInCharge = count[0].TookInChargeCount;
            var countNotTakeInCharge = count[0].NotTookInChargeCount;

            var nbPage = Math.Ceiling(
                Convert.ToDecimal(count[0].TotalResult) / Convert.ToDecimal(_nbResultPerPage));
            return Json(
                new
                {
                    clients = clients,
                    CountNotDefined = countNotDefined,
                    CountTookInCharge = countTookInCharge,
                    CountNotTakeInCharge = countNotTakeInCharge,
                    nbPage = nbPage
                });
        }
        public ActionResult DownloadExcelAssociationClient(DateTime? startDate, DateTime? endDate, int? statusId, int? associationId)
        {
            var isFrench = CultureInfo.CurrentCulture.Name == "fr-CA";

            if (endDate != null)
                endDate = endDate.Value.AddDays(1).AddSeconds(-1);

            var clients = _context.GetClientSentToExternalAssociation(
                true,
                startDate,
                endDate,
                statusId,
                associationId,
                1,
                9_999_999)
                .ToList();

            foreach (var client in clients)
            {
                client.ClientName = client.ClientName.Decode();
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage())
            {
                // Crée la feuille de travail "Liste de prix"
                string excelSheetName = Resource.ExternalAssociation_ExcelDocument_TabName;
                excel.Workbook.Worksheets.Add(excelSheetName);

                // Insère l'information sur les paramêtres utiliser pour générer la liste de prix
                List<string[]> lstExcellRow = new List<string[]>();
                int rowStartAt = 2;
                lstExcellRow.Add(new string[] {
                    Resource.ExternalAssociation_ExcelDocumentTableHeader_Association,
                    Resource.ExternalAssociation_ExcelDocumentTableHeader_Client,
                    Resource.ExternalAssociation_ExcelDocumentTableHeader_SentDate,
                    Resource.ExternalAssociation_ExcelDocumentTableHeader_StatusChangedDate,
                    Resource.ExternalAssociation_ExcelDocumentTableHeader_Status});

                // Determine les colonnes où insérer l'info (e.g. A1:D1)
                string rowRange = "B" + rowStartAt + ":" + Char.ConvertFromUtf32(lstExcellRow[0].Length + 64) + rowStartAt++;

                // Récupère la feuille de travail
                ExcelWorksheet worksheet = excel.Workbook.Worksheets[excelSheetName];

                // Insère la ligne dans la feuille de travail
                worksheet.Cells[rowRange].LoadFromArrays(lstExcellRow);

                clients.ForEach(c =>
                {
                    lstExcellRow.Add(new string[] {
                        c.AssociationName,
                        c.ClientName,
                        c.SentToAssociationDate.ToString("yyyy-MM-dd"),
                        c.StatusChangedDate.ToString("yyyy-MM-dd"),
                        c.Status});
                    rowStartAt = 2;

                    // Determine les colonnes où insérer l'info (e.g. A1:D1)
                    rowRange = "B" + rowStartAt + ":" + Char.ConvertFromUtf32(lstExcellRow[0].Length + 64) + rowStartAt++;

                    // Insère la ligne dans la feuille de travail
                    worksheet.Cells[rowRange].LoadFromArrays(lstExcellRow);
                });


                //Styling the worksheet
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                worksheet.Row(2).Style.Font.Bold = true;
                worksheet.Row(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                worksheet.Cells["B2:G2"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Select("B2");

                string excelName = DateTime.Today.ToString("yyyy_MM_dd") + "_Client_Association_Externe";

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }


            return Json(new { msg = "success" });
        }

        #endregion

        #region Appointment List


        public ActionResult AppointmentList()
        {
            var viewModel = new AppointmentListViewModel(_context);
            return View(viewModel);
        }


        public ActionResult LoadAppointmentList(bool isFrench, DateTime? startDate, DateTime? endDate, int? statusId, int? therapistId, string reclamationStatusValue, int page)
        {
            reclamationStatusValue = reclamationStatusValue == "null" ? null : reclamationStatusValue;


            if (endDate != null)
            {
                //On veut inclure la derniere journée
                endDate = endDate.Value.AddDays(1).AddSeconds(-1);
            }

            var appointments = _context.GetListAppointmentForAdmin(
                isFrench,
                startDate,
                endDate,
                statusId,
                therapistId,
                reclamationStatusValue,
                page,
                WebSiteProperties.NbResultPerPage)
                .ToList();

            if (!isFrench)
            {
                Thread.CurrentThread.CurrentCulture =
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-CA");
            }

            var resourceManager = new ResourceManager(typeof(Resource));

            foreach (var appointment in appointments)
            {
                appointment.AppointmentReclamationStatus = resourceManager.GetString(appointment.AppointmentReclamationStatus);
            }

            var appointmentCount = _context.GetListAppointmentForAdminCount(
                startDate,
                endDate,
                statusId,
                therapistId,
                reclamationStatusValue)
                .ToList();

            var nbPage =
                Convert.ToInt32(
                    Math.Ceiling(
                        Convert.ToDecimal(appointmentCount[0].TotalResult)
                        /
                        Convert.ToDecimal(WebSiteProperties.NbResultPerPage)
                    )
                );

            return Json
                (
                    new
                    {
                        appointments = appointments,
                        appointmentCount = appointmentCount[0],
                        nbPage = nbPage
                    }
                );
        }


        public ActionResult DownloadExcelAssociationList(DateTime? startDate, DateTime? endDate, int? appointmentStatusId, int? therapistId, string reclamationStatus)
        {
            reclamationStatus = reclamationStatus == "null" ? null : reclamationStatus;

            var isFrench = CultureInfo.CurrentCulture.Name == "fr-CA";

            if (endDate != null)
            {
                //On veut inclure la derniere journée
                endDate = endDate.Value.AddDays(1).AddSeconds(-1);
            }

            var appointments = _context.GetListAppointmentForAdmin(
                isFrench,
                startDate,
                endDate,
                appointmentStatusId,
                therapistId,
                reclamationStatus,
                1,
                9_999_999)
                .ToList();

            foreach (var appointment in appointments)
            {
                appointment.TherapistName = appointment.TherapistName.Decode();
                appointment.ClientName = appointment.ClientName.Decode();
            }

            var resourceManager = new ResourceManager(typeof(Resource));

            foreach (var appointment in appointments)
                appointment.AppointmentReclamationStatus = resourceManager.GetString(appointment.AppointmentReclamationStatus);


            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage())
            {
                // Crée la feuille de travail "Liste de prix"
                string excelSheetName = Resource.AppointmentList_ExcelDocument_TabName;
                excel.Workbook.Worksheets.Add(excelSheetName);

                // Insère l'information sur les paramêtres utiliser pour générer la liste de prix
                List<string[]> lstExcellRow = new List<string[]>();
                int rowStartAt = 2;
                lstExcellRow.Add(new string[] {
                    Resource.AppointmentList_ExcelDocumentTableHeader_Therapist,
                    Resource.AppointmentList_ExcelDocumentTableHeader_Date,
                    Resource.AppointmentList_ExcelDocumentTableHeader_Time,
                    Resource.AppointmentList_ExcelDocumentTableHeader_Client,
                    Resource.AppointmentList_ExcelDocumentTableHeader_Status,
                    Resource.AppointmentList_ExcelDocumentTableHeader_ReclamationStatus});

                // Determine les colonnes où insérer l'info (e.g. A1:D1)
                string rowRange = "B" + rowStartAt + ":" + Char.ConvertFromUtf32(lstExcellRow[0].Length + 64) + rowStartAt++;

                // Récupère la feuille de travail
                ExcelWorksheet worksheet = excel.Workbook.Worksheets[excelSheetName];

                // Insère la ligne dans la feuille de travail
                worksheet.Cells[rowRange].LoadFromArrays(lstExcellRow);

                appointments.ForEach(c =>
                {
                    lstExcellRow.Add(new string[] {
                        c.TherapistName,
                        c.StartDateTime.ToString("yyyy-MM-dd"),
                        c.StartDateTime.ToString("HH:mm"),
                        c.ClientName,
                        c.AppointmentStatus,
                        c.AppointmentReclamationStatus});
                    rowStartAt = 2;

                    // Determine les colonnes où insérer l'info (e.g. A1:D1)
                    rowRange = "B" + rowStartAt + ":" + Char.ConvertFromUtf32(lstExcellRow[0].Length + 64) + rowStartAt++;

                    // Insère la ligne dans la feuille de travail
                    worksheet.Cells[rowRange].LoadFromArrays(lstExcellRow);
                });


                //Styling the worksheet
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                worksheet.Row(2).Style.Font.Bold = true;
                worksheet.Row(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                worksheet.Cells["B2:G2"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Select("B2");

                string excelName = DateTime.Today.ToString("yyyy_MM_dd") + "_Liste_Rendez-vous";

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }



            return Json(new { msg = "success" });
        }

        #endregion

        #region Appointment Calendar


        public ActionResult AppointmentCalendar()
        {
            return View();
        }

        #endregion

        #region Client receipt

        public ActionResult ClientReceipt()
        {
            var viewModel = new ListClientReceiptViewModel(_context);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ClientReceiptApplyFilter(DateTime? startDate, DateTime? endDate, string clientName, int? therapistId, int pageNumber)
        {
            if (endDate.HasValue)
                endDate = endDate.Value.AddDays(1).AddSeconds(-1);

            var clientReceipt = _context.GetListClientReceipt(
                startDate,
                endDate,
                clientName.ToLower(),
                therapistId,
                pageNumber,
                _nbResultPerPage)
                .ToList();

            var count = _context.GetListClientReceiptCount(
                startDate,
                endDate,
                clientName.ToLower(),
                therapistId)
                .ToList();

            var nbPage = Math.Ceiling(
                Convert.ToDecimal(count[0]) / Convert.ToDecimal(_nbResultPerPage));

            return Json(
                new
                {
                    clientReceipt = clientReceipt,
                    nbPage = nbPage
                });
        }


        [HttpPost]
        public ActionResult ResendReceiptToClient(string emailClient, int clientAppointmentId)
        {

            var resendReceipt = new ResendClientReceipt();
            resendReceipt.ResendReceiptByEmail(emailClient, clientAppointmentId, _context);
            return Json(new { success = true });
        }


        [HttpPost]
        public ActionResult DownloadClientReceipt(int clientAppointmentId)
        {

            bool isFrench = CultureInfo.CurrentCulture.Name == "fr-CA";
            var clientAppointment = _context.ClientAppointments
                .Where(x => x.Id == clientAppointmentId)
                .First();

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

        [HttpPost]
        public ActionResult DownloadExcelClientReceipt(DateTime? startDate, DateTime? endDate, string clientName, int? therapistId)
        {
            clientName = clientName == "null" ? null : clientName;

            var isFrench = CultureInfo.CurrentCulture.Name == "fr-CA";

            if (endDate != null)
            {
                //On veut inclure la derniere journée
                endDate = endDate.Value.AddDays(1).AddSeconds(-1);
            }

            var clientReceipt = _context.GetClientReceiptsForExcel(
                startDate,
                endDate,
                clientName.ToLower(),
                therapistId,
                isFrench)
                .ToList();

            foreach (var appointment in clientReceipt)
            {
                appointment.ClientName = appointment.ClientName.Decode();
                appointment.TherapistName = appointment.TherapistName.Decode();
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage())
            {
                // Crée la feuille de travail "Liste de prix"
                string excelSheetName = Resource.ClientReceipt_ExcelDocument_TabName;
                excel.Workbook.Worksheets.Add(excelSheetName);

                // Insère l'information sur les paramêtres utiliser pour générer la liste de prix
                List<string[]> lstExcellRow = new List<string[]>();
                int rowStartAt = 2;
                lstExcellRow.Add(new string[]
                {
                    Resource.ClientReceipt_ExcelDocument_Date,
                    Resource.ClientReceipt_ExcelDocument_ClientName,
                    Resource.ClientReceipt_ExcelDocument_ReceiptNumber,
                    Resource.ClientReceipt_ExcelDocument_AppointmentType,
                    Resource.ClientReceipt_ExcelDocument_ConsultationType,
                    Resource.ClientReceipt_ExcelDocument_SubTotal,
                    Resource.ClientReceipt_ExcelDocument_Tps,
                    Resource.ClientReceipt_ExcelDocument_Tvq,
                    Resource.ClientReceipt_ExcelDocument_Total,
                    Resource.ClientReceipt_ExcelDocument_Therapist,
                });

                // Determine les colonnes où insérer l'info (e.g. A1:D1)
                string rowRange = "B" + rowStartAt + ":" + Char.ConvertFromUtf32(lstExcellRow[0].Length + 64) + rowStartAt++;

                // Récupère la feuille de travail
                ExcelWorksheet worksheet = excel.Workbook.Worksheets[excelSheetName];

                // Insère la ligne dans la feuille de travail
                worksheet.Cells[rowRange].LoadFromArrays(lstExcellRow);

                foreach (var receipt in clientReceipt)
                {
                    string appointmentType = string.Format(Resource.ClientReceiptPdf_Name_AppointmentType,
                        receipt.NatureAct,
                        receipt.Secteur);

                    var getAppointmentCost = new GetAppointmentCost(_context, receipt.Id);

                    lstExcellRow.Add(new string[]
                   {
                        receipt.AppointmentDate.ToString("yyyy-MM-dd"),
                        receipt.ClientName,
                        "#" + receipt.ClientPaymentReceiptNumber.ToString().PadLeft(6, '0'),
                        appointmentType,
                        receipt.ConsultationType,
                        getAppointmentCost.SubTotal.ToString("C"),
                        getAppointmentCost.Tps.ToString("C"),
                        getAppointmentCost.Tvq.ToString("C"),
                        getAppointmentCost.Total.ToString("C"),
                        receipt.TherapistName
                   });
                    rowStartAt = 2;

                    // Determine les colonnes où insérer l'info (e.g. A1:D1)
                    rowRange = "B" + rowStartAt + ":" + Char.ConvertFromUtf32(lstExcellRow[0].Length + 64) + rowStartAt++;

                    // Insère la ligne dans la feuille de travail
                    worksheet.Cells[rowRange].LoadFromArrays(lstExcellRow);
                }


                //Styling the worksheet
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                worksheet.Row(2).Style.Font.Bold = true;
                worksheet.Row(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                worksheet.Cells["B2:K2"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Select("B2");

                string excelName = DateTime.Today.ToString("yyyy_MM_dd") + "_Liste_Recu_Client";

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }

            return Json(new { msg = "success" });
        }

        #endregion

        #region Therapist bill

        public ActionResult TherapistBill()
        {
            var viewModel = new ListTherapistBillViewModel(_context);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult TherapistBillApplyFilter(DateTime? startDate, DateTime? endDate, bool? isPaid, int? therapistId, int pageNumber)
        {
            if (endDate != null)
                endDate = endDate.Value.AddDays(1).AddSeconds(-1);

            var therapistbill = _context.GetListTherapistBillForAdmin(
                startDate,
                endDate,
                isPaid,
                therapistId,
                pageNumber,
                _nbResultPerPage)
                .ToList();

            var count = _context.GetListTherapistBillForAdminCount(
                startDate,
                endDate,
                isPaid,
                therapistId)
                .ToList();

            var nbPage = Math.Ceiling(
                Convert.ToDecimal(count[0]) / Convert.ToDecimal(_nbResultPerPage));

            return Json(
                new
                {
                    therapistbill = therapistbill,
                    nbPage = nbPage
                });
        }


        [HttpPost]
        public ActionResult TherapistBillSetPaymentDate(int therapistBillId, DateTime paymentDate)
        {

            // DateTime paymentDateBd = DateTime.Parse(paymentDate);

            var therapistBill = _context.TherapistBill
                .Where(x => x.Id == therapistBillId)
                .Single();

            therapistBill.PaymentDate = paymentDate;

            _context.SaveChanges();

            return Json(new { success = true });
        }


        [HttpPost]
        public ActionResult DownloadTherapistBill(int therapistBillId)
        {
            string pageHtmlPathUrl = Server.MapPath("/PDF/TherapistBillTemplate.html");
            string[] pageContentHtml = System.IO.File.ReadAllLines(pageHtmlPathUrl);

            var therapistBill = _context.TherapistBill
                .Where(x => x.Id == therapistBillId)
                .First();

            //Get the list of appointment from that bill
            var clientAppointments = _context.ClientAppointments
                .Where(x => x.TherapistId == therapistBill.Therapists.Id && x.TherapistBillId == therapistBill.BillNumber)
                .ToList();

            var generateTherapistBill = new GenerateTherapistBillPdf(pageContentHtml, clientAppointments, _context, therapistBill);


            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + generateTherapistBill.PdfName + ".pdf");
            Response.BinaryWrite(generateTherapistBill.PdfBuffer);
            Response.End();


            return Json(new { success = "success" });

        }

        [HttpPost]
        public ActionResult DownloadExcelTherapistBill(DateTime? startDate, DateTime? endDate, string exportationType, int? associationId)
        {
            if (endDate != null)
                endDate = endDate.Value.AddDays(1).AddSeconds(-1);

            var isFrench = CultureInfo.CurrentCulture.Name == "fr-CA";



            //Set if it is a payment exportation, if not, it is bill exportation
            bool isTherapistBillPayment = exportationType == "payment";

            var therapistBill = _context.GetListTherapistBillSentForAdminExcel(
                startDate,
                endDate).ToList();

            var therapistPayment = _context.GetListTherapistBillPaymentForAdminExcel(
                startDate,
                endDate).ToList();





            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage())
            {
                // Crée la feuille de travail "Liste de prix"
                string excelSheetName;


                // Insère l'information sur les paramêtres utiliser pour générer la liste de prix
                List<string[]> lstExcellRow = new List<string[]>();
                int rowStartAt = 2;


                //There is an additionnal column when it is a payment
                if (isTherapistBillPayment)
                {
                    //It is a payment exportation
                    excelSheetName = Resource.TherapistBill_ExcelDocumentTableHeader_Payment_TabName;
                    lstExcellRow.Add(new string[] {
                        Resource.TherapistBill_ExcelDocumentTableHeader_TherapistName,
                        Resource.TherapistBill_ExcelDocumentTableHeader_BillNumber,
                        Resource.TherapistBill_ExcelDocumentTableHeader_SubTotal,
                        Resource.TherapistBill_ExcelDocumentTableHeader_TpsAmount,
                        Resource.TherapistBill_ExcelDocumentTableHeader_TvqAmount,
                        Resource.TherapistBill_ExcelDocumentTableHeader_TotalBill,
                        Resource.TherapistBill_ExcelDocumentTableHeader_TherapistTpsNumber,
                        Resource.TherapistBill_ExcelDocumentTableHeader_TherapistTvqNumber,
                        Resource.TherapistBill_ExcelDocumentTableHeader_BillSentDate,
                        Resource.TherapistBill_ExcelDocumentTableHeader_BillPaymentDate});
                }
                else
                {
                    //It is a bill exportation
                    excelSheetName = Resource.TherapistBill_ExcelDocumentTableHeader_Bill_TabName;
                    lstExcellRow.Add(new string[] {
                    Resource.TherapistBill_ExcelDocumentTableHeader_TherapistName,
                    Resource.TherapistBill_ExcelDocumentTableHeader_BillNumber,
                    Resource.TherapistBill_ExcelDocumentTableHeader_SubTotal,
                    Resource.TherapistBill_ExcelDocumentTableHeader_TpsAmount,
                    Resource.TherapistBill_ExcelDocumentTableHeader_TvqAmount,
                    Resource.TherapistBill_ExcelDocumentTableHeader_TotalBill,
                    Resource.TherapistBill_ExcelDocumentTableHeader_TherapistTpsNumber,
                    Resource.TherapistBill_ExcelDocumentTableHeader_TherapistTvqNumber,
                    Resource.TherapistBill_ExcelDocumentTableHeader_BillSentDate});
                }
                excel.Workbook.Worksheets.Add(excelSheetName);

                // Determine les colonnes où insérer l'info (e.g. A1:D1)
                string rowRange = "B" + rowStartAt + ":" + Char.ConvertFromUtf32(lstExcellRow[0].Length + 64) + rowStartAt++;

                // Récupère la feuille de travail
                ExcelWorksheet worksheet = excel.Workbook.Worksheets[excelSheetName];

                // Insère la ligne dans la feuille de travail
                worksheet.Cells[rowRange].LoadFromArrays(lstExcellRow);


                //Payment exportation
                if (isTherapistBillPayment)
                {
                    foreach (var payment in therapistPayment)
                    {
                        lstExcellRow.Add(new string[] {
                        payment.TherapistName,
                        "RSPYF-" + payment.BillNumber,
                        string.Format("{0:n}", payment.SubTotal),
                        string.Format("{0:n}", payment.TpsAmount),
                        string.Format("{0:n}", payment.TvqAmount),
                        string.Format("{0:n}", payment.SubTotal + payment.TpsAmount + payment.TvqAmount),
                        payment.TpsNumber,
                        payment.TvqNumber,
                        payment.BillSendDate.ToString("yyyy-MM-dd"),
                        payment.PaymentDate.Value.ToString("yyyy-MM-dd")});
                        rowStartAt = 2;

                        // Determine les colonnes où insérer l'info (e.g. A1:D1)
                        rowRange = "B" + rowStartAt + ":" + Char.ConvertFromUtf32(lstExcellRow[0].Length + 64) + rowStartAt++;

                        // Insère la ligne dans la feuille de travail
                        worksheet.Cells[rowRange].LoadFromArrays(lstExcellRow);
                    }
                }

                //Bill exportation
                else
                {
                    therapistBill.ForEach(c =>
                    {
                        lstExcellRow.Add(new string[] {
                        c.TherapistName,
                        "RSPYF-" + c.BillNumber,
                        string.Format("{0:n}", c.SubTotal),
                        string.Format("{0:n}", c.TpsAmount),
                        string.Format("{0:n}", c.TvqAmount),
                        string.Format("{0:n}", c.SubTotal + c.TpsAmount + c.TvqAmount),
                        c.TpsNumber,
                        c.TvqNumber,
                        c.BillSendDate.ToString("yyyy-MM-dd")});
                        rowStartAt = 2;

                        // Determine les colonnes où insérer l'info (e.g. A1:D1)
                        rowRange = "B" + rowStartAt + ":" + Char.ConvertFromUtf32(lstExcellRow[0].Length + 64) + rowStartAt++;

                        // Insère la ligne dans la feuille de travail
                        worksheet.Cells[rowRange].LoadFromArrays(lstExcellRow);
                    });
                }



                //Styling the worksheet
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                worksheet.Row(2).Style.Font.Bold = true;
                worksheet.Row(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                string excelName = "";

                if (isTherapistBillPayment)
                {
                    worksheet.Cells["B2:K2"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    excelName = DateTime.Today.ToString("yyyy_MM_dd") + "_Therapist_Payment";
                }
                else
                {
                    worksheet.Cells["B2:J2"].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    excelName = DateTime.Today.ToString("yyyy_MM_dd") + "_Therapist_Bill";
                }

                worksheet.Select("B2");

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                    excel.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }


            return Json(new { msg = "success" });
        }


        #endregion

        #region Statistics


        public ActionResult Statistic()
        {
            var viewModel = new StatisticsViewModel(_context);
            return View(viewModel);
        }


        [HttpPost]
        public ActionResult StatisticApplyFilter(DateTime? startDate, DateTime? endDate)
        {
            if (endDate != null)
                endDate = endDate.Value.AddDays(1).AddSeconds(-1);

            var isFrench = CultureInfo.CurrentCulture.Name == "fr-CA" ? true : false;

            var therapistClosingRate = _context.GetTherapistClosingRates(
                startDate,
                endDate).ToList();

            var cancelationReason = _context.GetCancelationCountByCancelationReason(
                isFrench,
                startDate,
                endDate).ToList();

            int totalCanceldAppointment = 0;

            foreach (var cancelation in cancelationReason)
            {
                totalCanceldAppointment += cancelation.NbCancelation.Value;
            }

            return Json(new
            {
                therapistClosingRate = therapistClosingRate,
                cancelationReason = cancelationReason,
                totalCanceldAppointment = totalCanceldAppointment
            });
        }

        #endregion

        #region Dashboard

        public ActionResult Dashboard()
        {
            var viewModel = new DashboardViewModel(_context);
            return View(viewModel);
        }

        #endregion

        #region Client List

        public ActionResult ClientList()
        {
            var viewModel = new ClientListViewModel(_context);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ClientListApplyFilter(int? therapistId, int pageNumber)
        {
            var clients = _context.GetClientListForAdminClientListPage(
                therapistId,
                pageNumber,
                WebSiteProperties.NbResultPerPage)
                .ToList();

            var nbResult = _context.GetClientListForAdminClientListPageCount(therapistId).First();

            var nbPage =
                Convert.ToInt32(
                    Math.Ceiling(
                        Convert.ToDecimal(nbResult)
                        /
                        Convert.ToDecimal(WebSiteProperties.NbResultPerPage)
                    )
                );

            return Json
                (
                    new
                    {
                        clients = clients,
                        nbPage = nbPage
                    }
                );
        }

        #endregion

        #region Detail Client

        public ActionResult DetailClient(int? clientId)
        {
            DetailClientViewModel viewModel;
            if (clientId != null)
            {
                viewModel = new DetailClientViewModel(_context, clientId.Value);
            }
            else
            {
                viewModel = new DetailClientViewModel(_context);
            }

            if (!viewModel.CanAccess)
                return RedirectToAction("ClientList");

            return View(viewModel);
        }

        public ActionResult CreateClient(DetailClientDatas datas)
        {

            #region Verify the datas

            if (datas.FirstName.Trim() == "" ||
                datas.LastName.Trim() == "" ||
                datas.Email.Trim() == "")
            {
                throw new Exception();
            }

            #endregion

            var createClient = new CreateClient(_context, datas);
            if (!createClient.Succeeded)
            {
                //La création du thérapeute a échoué, on retourne le form
                throw new Exception();
            }

            TempData["Message"] = Resource.DetailClientAdmin_SucceededCreated;

            return Json(new { result = "Redirect", url = Url.Action("ClientList", "admin") });

        }

        public ActionResult EditClient(DetailClientDatas datas)
        {
            #region Verify the datas

            if (datas.FirstName.Trim() == "" ||
                datas.LastName.Trim() == "" ||
                datas.Email.Trim() == "")
            {
                throw new Exception();
            }

            #endregion

            var editClient = new EditClient(_context, datas, datas.ClientId.Value);
            if (!editClient.Succeeded)
            {
                //La modification du thérapeute a échoué, on renvoie le form
                throw new Exception();
            }

            return Json(new { success = true });
        }


        #endregion



    }
}