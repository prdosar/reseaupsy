using Business;
using ReseauPsy.ViewModel.Therapist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using ReseauPsy.Models;
using Library.Helper;
using ReseauPsy.ViewModel.Admin;
using ReseauPsy.Models.Therapist;
using System.Globalization;
using HelpersLibrary;
using OfficeOpenXml;
using ReseauPsy.Resources;
using System.Resources;
using System.IO;
using TuesPechkin;
using System.Drawing.Printing;
using ReseauPsy.PDF.Configuration;
using System.Web.Routing;

namespace ReseauPsy.Controllers.Therapist
{
    [Authorize(Roles = RoleName.Therapist)]
    [RedirectAttribute]
    public class TherapistController : Controller
    {

        private ReseauPsyEntities _context;

        //Constructor
        public TherapistController()
        {
            _context = new ReseauPsyEntities();
        }


        private Business.Therapist _therapistObject
        {
            get
            {
                string userIdentity = User.Identity.GetUserId();
                return _context.Therapists
                    .Where(x => x.AspNetUserId == userIdentity)
                    .First();
            }
        }

        private int _nbResultPerPage
        {
            get
            {
                return WebSiteProperties.NbResultPerPage;
            }
        }

        #region Mon profil

        // GET: Therapist
        public ActionResult MyProfile()
        {
            var viewModel = new MyProfileViewModel(_context, _therapistObject);
            return View("MyProfile", viewModel);
        }

        public ActionResult ModifyProfile(TherapistProfileDatas datas)
        {
            #region Verify datas

            if (datas.LastName.Trim() == "" ||
                datas.FirstName.Trim() == "" ||
                datas.Email.Trim() == "" ||
                datas.PhoneNumber.Trim() == "" ||
                datas.Adress.Trim() == "" ||
                datas.City.Trim() == "" ||
                datas.PostalCode.Trim() == "")
            {
                throw new Exception();
            }

            if ((datas.AccreditationId == 1 || datas.AccreditationId == 2) && datas.OpqNumber.Trim() == "")
            {
                throw new Exception();
            }

            var therapistInfo = new GetTherapistInfo(_context, _therapistObject.Id);
            therapistInfo.GetMostRecentTherapistInfo();

            if (therapistInfo.IsTaxable && (datas.TpsNumber.Trim() == "" || datas.TvqNumber.Trim() == ""))
            {
                throw new Exception();
            }

            #endregion

            datas.TherapistId = _therapistObject.Id;

            #region On verifie le courriel

            if (therapistInfo.Email != datas.Email)
            {
                //On change le email. On doit vérifier si le nouveau courriel n'est pas en utilisation
                if (!VerifyIdentityEmail.VerifyEmailAvailabillity(_context, datas.Email))
                {
                    //Le courriel est deja en utilisation, on renvoie le form
                    return Json(new { success = false, error = "EmailUsed" });
                }
            }

            #endregion

            bool isRedirectToDashboard = false;

            if (!_context.VerifiyIfTherapistInscriptionCompleted(_therapistObject.Id)
                .Select(x => x.IsInscriptionCompleted)
                .First())
            {
                isRedirectToDashboard = true;
            }

            var editProdile = new EditProfile(datas, _context, _therapistObject);
            if (!editProdile.Succeeded)
            {
                //La modification du thérapeute a échoué, on renvoie le form
                throw new Exception();
            }

            if (isRedirectToDashboard)
                return Json(new { result = "Redirect", url = Url.Action("Dashboard", "therapist") });

            return Json(new { success = true });
        }
        #endregion

        #region Mes competences

        public ActionResult MySkills()
        {
            var viewModel = new MySkillsViewModel(_context, _therapistObject);
            return View("MySkills", viewModel);
        }

        [HttpPost]
        public ActionResult AddAssociation(string associationNameNumber)
        {
            var association = new TherapistAssociationAndProfessionalOrder();
            association.Therapist = _therapistObject;

            var associationNameNumberSplitted = associationNameNumber.Split(
                new string[] { "--split--" },
                StringSplitOptions.None);

            association.AssociationName = associationNameNumberSplitted[0];
            association.AssociationNumber = associationNameNumberSplitted[1];
            association.CreateDate = DateTime.Now;
            _therapistObject.TherapistAssociationAndProfessionalOrders.Add(association);
            try
            {
                _context.SaveChanges();

                return Json(new { associationId = association.Id });
            }
            catch (Exception)
            {

                return Json(new { msg = "failed" });
            }
        }

        [HttpPost]
        public ActionResult DeleteAssociation(int therapistAssociationId)
        {

            var association = _context.TherapistAssociationAndProfessionalOrders
                .Single(x => x.Id == therapistAssociationId);

            association.DeleteDate = DateTime.Now;

            try
            {
                _context.SaveChanges();
                return Json(new { msg = "success" });
            }
            catch (Exception)
            {
                return Json(new { msg = "failed" });
            }
        }

        [HttpPost]
        public ActionResult ModifyAssociation(int therapistAssociationId, string associationNameNumber)
        {
            var association = _context.TherapistAssociationAndProfessionalOrders
                .Single(x => x.Id == therapistAssociationId);

            association.DeleteDate = DateTime.Now;

            var associationNameNumberSplitted = associationNameNumber.Split(
                new string[] { "--split--" },
                StringSplitOptions.None);

            var newAssociation = new TherapistAssociationAndProfessionalOrder();
            newAssociation.Therapist = _therapistObject;
            newAssociation.AssociationName = associationNameNumberSplitted[0];
            newAssociation.AssociationNumber = associationNameNumberSplitted[1];
            newAssociation.CreateDate = DateTime.Now;

            _therapistObject.TherapistAssociationAndProfessionalOrders.Add(newAssociation);
            try
            {
                _context.SaveChanges();
                return Json(new { msg = "success" });
            }
            catch (Exception)
            {
                return Json(new { msg = "failed" });
            }
        }


        [HttpPost]
        public ActionResult AddService(int serviceId, string serviceTypeIds)
        {
            int[] typeIds = Array.ConvertAll(serviceTypeIds.Split(','), s => int.Parse(s));
            foreach (var typeId in typeIds)
            {
                var offeredserviceofferedservicetype = _context.OfferedService_OfferedServiceType
                    .Where(x => x.OfferedServiceId == serviceId && x.OfferedServiceTypeId == typeId)
                    .Single();

                var therpistOfferedService = new TherapistOfferedService();
                therpistOfferedService.Therapist = _therapistObject;
                therpistOfferedService.OfferedService_OfferedServiceType = offeredserviceofferedservicetype;
                _therapistObject.TherapistOfferedServices.Add(therpistOfferedService);
            }

            try
            {
                _context.SaveChanges();
                return Json(new { msg = "success" });
            }
            catch (Exception)
            {
                return Json(new { msg = "Failed" });
            }
        }

        [HttpPost]
        public ActionResult DeleteService(int serviceId, string serviceTypeIds)
        {
            int[] typeIds = Array.ConvertAll(serviceTypeIds.Split(','), s => int.Parse(s));

            foreach (var typeId in typeIds)
            {
                var offeredserviceofferedservicetype = _context.OfferedService_OfferedServiceType
                    .Where(x => x.OfferedServiceId == serviceId && x.OfferedServiceTypeId == typeId)
                    .Single();

                var therapistService = _context.TherapistOfferedServices
                    .Where(x => x.TherapistId == _therapistObject.Id && x.OfferedService_OfferedServiceTypeId == offeredserviceofferedservicetype.Id)
                    .SingleOrDefault();

                _context.TherapistOfferedServices.Remove(therapistService);
            }

            try
            {
                _context.SaveChanges();
                return Json(new { msg = "success" });
            }
            catch (Exception)
            {
                return Json(new { msg = "Failed" });
            }
        }

        [HttpPost]
        public ActionResult ModifyService(int previousServiceId, string previousServiceTypeIds, int serviceId, string serviceTypeIds)
        {
            int[] previousTypeIds = Array.ConvertAll(previousServiceTypeIds.Split(','), s => int.Parse(s));

            foreach (var typeId in previousTypeIds)
            {
                var offeredserviceofferedservicetype = _context.OfferedService_OfferedServiceType
                    .Where(x => x.OfferedServiceId == previousServiceId && x.OfferedServiceTypeId == typeId)
                    .Single();

                var therapistService = _context.TherapistOfferedServices
                    .Where(x => x.TherapistId == _therapistObject.Id && x.OfferedService_OfferedServiceTypeId == offeredserviceofferedservicetype.Id)
                    .SingleOrDefault();

                _context.TherapistOfferedServices.Remove(therapistService);
            }

            int[] TypeIds = Array.ConvertAll(serviceTypeIds.Split(','), s => int.Parse(s));

            foreach (var typeId in TypeIds)
            {
                var offeredserviceofferedservicetype = _context.OfferedService_OfferedServiceType
                    .Where(x => x.OfferedServiceId == serviceId && x.OfferedServiceTypeId == typeId)
                    .Single();

                var therpistOfferedService = new TherapistOfferedService();
                therpistOfferedService.Therapist = _therapistObject;
                therpistOfferedService.OfferedService_OfferedServiceType = offeredserviceofferedservicetype;
                _therapistObject.TherapistOfferedServices.Add(therpistOfferedService);
            }

            try
            {
                _context.SaveChanges();
                return Json(new { msg = "success" });
            }
            catch (Exception)
            {
                return Json(new { msg = "Failed" });
            }
        }

        [HttpPost]
        public ActionResult AddPracticeArea(int practiceId)
        {
            var therapistPracticeArea = new TherapistAreaOfPractice();
            therapistPracticeArea.Therapist = _therapistObject;
            therapistPracticeArea.AreaOfPracticeId = practiceId;

            _therapistObject.TherapistAreaOfPractices.Add(therapistPracticeArea);

            try
            {
                _context.SaveChanges();
                return Json(new { msg = "success" });
            }
            catch (Exception)
            {
                return Json(new { msg = "Failed" });
            }
        }

        [HttpPost]
        public ActionResult RemovePracticeArea(int practiceId)
        {

            var therapistPracticeArea = _context.TherapistAreaOfPractices
                .Where(x => x.TherapistId == _therapistObject.Id && x.AreaOfPracticeId == practiceId)
                .FirstOrDefault();

            _context.TherapistAreaOfPractices.Remove(therapistPracticeArea);

            try
            {
                _context.SaveChanges();
                return Json(new { msg = "success" });
            }
            catch (Exception)
            {
                return Json(new { msg = "Failed" });
            }
        }

        [HttpPost]
        public ActionResult ModifyPracticeArea(int previousPracticeId, int practiceId)
        {
            var therapistPractice = _context.TherapistAreaOfPractices
                .Where(x => x.TherapistId == _therapistObject.Id && x.AreaOfPracticeId == previousPracticeId)
                .Single();

            therapistPractice.AreaOfPracticeId = practiceId;

            try
            {
                _context.SaveChanges();
                return Json(new { msg = "success" });
            }
            catch (Exception)
            {
                return Json(new { msg = "Failed" });
            }
        }


        [HttpPost]
        public ActionResult AddOrientation(int orientationId)
        {
            var therapistOrientation = new TherapistTheoreticalOrientation();
            therapistOrientation.Therapist = _therapistObject;
            therapistOrientation.TheoreticalOrientationId = orientationId;

            _therapistObject.TherapistTheoreticalOrientations.Add(therapistOrientation);

            try
            {
                _context.SaveChanges();
                return Json(new { msg = "success" });
            }
            catch (Exception)
            {
                return Json(new { msg = "Failed" });
            }
        }

        [HttpPost]
        public ActionResult DeleteOrientation(int orientationId)
        {
            var therapistOrientation = _context.TherapistTheoreticalOrientations
                .Where(x => x.TherapistId == _therapistObject.Id && x.TheoreticalOrientationId == orientationId)
                .Single();

            _context.TherapistTheoreticalOrientations.Remove(therapistOrientation);

            try
            {
                _context.SaveChanges();
                return Json(new { msg = "success" });
            }
            catch (Exception)
            {
                return Json(new { msg = "Failed" });
            }
        }

        [HttpPost]
        public ActionResult ModifyOrientation(int previousOrientationId, int orientationId)
        {
            var therapistOrientation = _context.TherapistTheoreticalOrientations
                .Where(x => x.TherapistId == _therapistObject.Id && x.TheoreticalOrientationId == previousOrientationId)
                .Single();

            therapistOrientation.TheoreticalOrientationId = orientationId;

            try
            {
                _context.SaveChanges();
                return Json(new { msg = "success" });
            }
            catch (Exception)
            {
                return Json(new { msg = "Failed" });
            }
        }


        [HttpPost]
        public ActionResult AddSkill(int skillId)
        {
            var therapistSkill = new TherapistSpecificSkill();
            therapistSkill.Therapist = _therapistObject;
            therapistSkill.SpecificSkillId = skillId;

            _therapistObject.TherapistSpecificSkills.Add(therapistSkill);

            try
            {
                _context.SaveChanges();
                return Json(new { msg = "success" });
            }
            catch (Exception)
            {
                return Json(new { msg = "Failed" });
            }
        }

        [HttpPost]
        public ActionResult DeleteSkill(int skillId)
        {
            var therapistSkill = _context.TherapistSpecificSkills
                .Where(x => x.TherapistId == _therapistObject.Id && x.SpecificSkillId == skillId)
                .Single();

            _context.TherapistSpecificSkills.Remove(therapistSkill);

            try
            {
                _context.SaveChanges();
                return Json(new { msg = "success" });
            }
            catch (Exception)
            {
                return Json(new { msg = "Failed" });
            }
        }

        [HttpPost]
        public ActionResult ModifySkill(int previousSkillId, int skillId)
        {
            var therapistSkill = _context.TherapistSpecificSkills
                .Where(x => x.TherapistId == _therapistObject.Id && x.SpecificSkillId == previousSkillId)
                .Single();

            therapistSkill.SpecificSkillId = skillId;

            try
            {
                _context.SaveChanges();
                return Json(new { msg = "success" });
            }
            catch (Exception)
            {
                return Json(new { msg = "Failed" });
            }
        }

        #endregion

        #region Therapist Client Request

        public ActionResult TherapistClientRequest()
        {
            var viewModel = new TherapistClientRequestViewModel(_context, _therapistObject.Id);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AnswerClientRequest(bool isAccepted, int requestId)
        {
            var request = _context.TherapistClientRequest
                .Where(x => x.Id == requestId)
                .Single();

            request.IsAccepted = isAccepted;

            request.Clients.TherapistId = _therapistObject.Id;

            if (!isAccepted)
                request.Clients.TherapistId = null;

            try
            {
                _context.SaveChanges();
                return Json(new { msg = "success" });
            }
            catch (Exception)
            {
                throw new Exception();
            }

        }

        #endregion

        #region Appointment

        public ActionResult Appointment()
        {
            var viewModel = new AppointmentViewModel(_context, _therapistObject.Id);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddAppointments(AddAppointmentInfos AddAppointmentInfos)
        {
            //Get the appointment duration in minutes
            double durationMinute = Decimal.ToDouble(AddAppointmentInfos.NbSession * 60);

            //Get the datetime of the selected date
            DateTime.TryParseExact($"{AddAppointmentInfos.StartDate} {AddAppointmentInfos.TimeStart}",
                "dd-MM-yyyy HH:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime startDate);

            //thrwo exception if trhe date is smaller than today
            if (startDate < DateTime.Now)
                throw new Exception();

            //Un client qui n'est pas external devrait toujours avoir un wageId
            if (AddAppointmentInfos.WageId == null)
            {
                var client = _context.Clients
                    .Where(x => x.Id == AddAppointmentInfos.ClientId)
                    .Single();

                if (!client.IsExternalClient)
                    throw new Exception();
            }

            //The date of the first appointment to set the calendar on this date
            var firstAppointnment = startDate;

            //The url to pay the first appointment
            string firstGuidUrl = null;

            //The list of all the appointments already scheduled
            var appointmentsAlreadyScheduled = _context.ClientAppointments
                .Where(x => x.TherapistId == _therapistObject.Id && !x.IsDeleted)
                .ToList();

            //Check if the appointment can be saved
            var isTherapistFree = true;
            var conflicts = new List<AppointmentCalendarModel>();

            //Save the appointment here if there is a repetition
            if (AddAppointmentInfos.IsRepetition)
            {
                int repetitionId = _context.ClientAppointmentRepetitions.ToList().Count == 0
                    ? 1
                    : _context.ClientAppointmentRepetitions.Max(x => x.Id);

                var clientAppointmentRepetition = new ClientAppointmentRepetition();

                clientAppointmentRepetition.ReferenceDate = startDate;

                _context.ClientAppointmentRepetitions.Add(clientAppointmentRepetition);

                #region Set datetime of everyday if repeat everyweek

                //If the repeat is on one week, it might have more than one appointment
                //Per week. So we need to get the first day of each different day of the week
                //in order to do +7 later on to add the other appointment
                List<DateTime> firstAppointmentsDateTime = new List<DateTime>();
                if (AddAppointmentInfos.RepeatEveryPeriod == "week" && AddAppointmentInfos.RepeatEveryNumber == 1)
                {
                    //Sunday is 0 but we want it as 7
                    var startDayId = (int)startDate.DayOfWeek == 0 ? 7 : (int)startDate.DayOfWeek;
                    foreach (var dayId in AddAppointmentInfos.RepeatDays)
                    {
                        if (dayId == startDayId)
                            firstAppointmentsDateTime.Add(startDate);

                        else if (dayId > startDayId)
                            firstAppointmentsDateTime.Add(startDate.AddDays(dayId - startDayId));

                        else
                            firstAppointmentsDateTime.Add(startDate.AddDays(dayId - startDayId + 7));
                    }

                    firstAppointnment = firstAppointmentsDateTime.Min();
                }


                #endregion

                for (int i = 0; i < AddAppointmentInfos.RepeatAmount; i++)
                {
                    DateTime appointmentStartDate;

                    #region Set the start time of the appointment

                    if (AddAppointmentInfos.RepeatEveryPeriod == "month")
                        appointmentStartDate = startDate.AddMonths(i * Convert.ToInt32(AddAppointmentInfos.RepeatEveryNumber));

                    //Its on week and its not every week so we dont deal with days
                    else if (AddAppointmentInfos.RepeatEveryNumber != 1)
                        appointmentStartDate = startDate.AddDays(i * Convert.ToInt32(AddAppointmentInfos.RepeatEveryNumber) * 7);

                    //Every week, so we deal with the different week days
                    else
                    {
                        var nbRepeatPerWeek = firstAppointmentsDateTime.Count;
                        var dayToTake = i;
                        var dayToAdd = 0;

                        while (dayToTake > nbRepeatPerWeek - 1)
                        {
                            dayToTake = dayToTake - nbRepeatPerWeek;
                            dayToAdd = dayToAdd + 7;
                        }

                        appointmentStartDate = firstAppointmentsDateTime[dayToTake].AddDays(dayToAdd);
                    }

                    #endregion

                    DateTime appointmentEndDate = appointmentStartDate.AddMinutes(durationMinute);

                    #region Check if therapist is free

                    foreach (var clientAppointment in appointmentsAlreadyScheduled)
                    {
                        if ((appointmentStartDate >= clientAppointment.StartDateTime && appointmentStartDate < clientAppointment.EndDateTime) ||
                            (appointmentEndDate > clientAppointment.StartDateTime && appointmentEndDate <= clientAppointment.EndDateTime))
                        {

                            var clientInfos = new GetClientInfos(_context, clientAppointment.ClientId);
                            clientInfos.GetMostRecentClientInfo();

                            isTherapistFree = false;
                            conflicts.Add(new AppointmentCalendarModel
                            {
                                title = $"{clientInfos.FirstName.Decode()} {clientInfos.LastName.Decode()}",
                                start = clientAppointment.StartDateTime.ToString("dd MMM yyyy HH:mm"),
                                end = clientAppointment.EndDateTime.ToString("dd MMM yyyy HH:mm")
                            });
                        }
                    }

                    #endregion

                    var appointment = new ClientAppointments();
                    appointment.TherapistId = _therapistObject.Id;
                    appointment.ClientId = AddAppointmentInfos.ClientId;
                    appointment.StartDateTime = appointmentStartDate;
                    appointment.ReferenceStartDateTime = appointmentStartDate;
                    appointment.EndDateTime = appointmentEndDate;
                    appointment.NbSession = AddAppointmentInfos.NbSession;
                    appointment.TherapistPayInformationId = AddAppointmentInfos.WageId;
                    appointment.ClientAppointmentRepetition = clientAppointmentRepetition;
                    appointment.NatureActId = AddAppointmentInfos.NatureActId;
                    appointment.ProfessionalTitleId = AddAppointmentInfos.ProfessionnalTitleId;
                    appointment.ConsultationTypeId = AddAppointmentInfos.ConsultationTypeId;
                    appointment.AppointmentStatusId = 1; //Forthcoming
                    appointment.IsDeleted = false;
                    var guidUrl = Guid.NewGuid();
                    appointment.PaymentUrlCode = guidUrl;

                    if (firstGuidUrl == null)
                    {
                        firstGuidUrl = guidUrl.ToString();
                    }

                    _context.ClientAppointments.Add(appointment);
                }
            }
            //Only one appointment to save
            else
            {

                #region Check if therapist is free

                var endDate = startDate.AddMinutes(durationMinute);

                foreach (var clientAppointment in appointmentsAlreadyScheduled)
                {
                    if ((startDate >= clientAppointment.StartDateTime && startDate < clientAppointment.EndDateTime) ||
                        (endDate > clientAppointment.StartDateTime && endDate <= clientAppointment.EndDateTime) ||
                        (startDate < clientAppointment.StartDateTime && endDate > clientAppointment.EndDateTime))
                    {

                        var clientInfos = new GetClientInfos(_context, clientAppointment.ClientId);
                        clientInfos.GetMostRecentClientInfo();

                        isTherapistFree = false;
                        conflicts.Add(new AppointmentCalendarModel
                        {
                            title = $"{clientInfos.FirstName.Decode()} {clientInfos.LastName.Decode()}",
                            start = clientAppointment.StartDateTime.ToString("dd MMM yyyy HH:mm"),
                            end = clientAppointment.EndDateTime.ToString("dd MMM yyyy HH:mm")
                        });
                    }
                }

                #endregion

                var appointment = new ClientAppointments();
                appointment.TherapistId = _therapistObject.Id;
                appointment.ClientId = AddAppointmentInfos.ClientId;
                appointment.StartDateTime = startDate;
                appointment.NbSession = AddAppointmentInfos.NbSession;
                appointment.TherapistPayInformationId = AddAppointmentInfos.WageId;
                appointment.EndDateTime = endDate;
                appointment.NatureActId = AddAppointmentInfos.NatureActId;
                appointment.ProfessionalTitleId = AddAppointmentInfos.ProfessionnalTitleId;
                appointment.ConsultationTypeId = AddAppointmentInfos.ConsultationTypeId;
                appointment.AppointmentStatusId = 1; //Forthcoming
                appointment.IsDeleted = false;
                var guidUrl = Guid.NewGuid();
                appointment.PaymentUrlCode = guidUrl;

                firstGuidUrl = guidUrl.ToString();

                _context.ClientAppointments.Add(appointment);
            }

            //Block the treatment if there is a conflict
            if (!isTherapistFree)
            {
                return Json(new
                {
                    isSuccess = false,
                    conflicts = conflicts
                });
            }

            try
            {
                _context.SaveChanges();
                return Json(new
                {
                    isSuccess = true,
                    firstAppointnment = firstAppointnment
                });
            }
            catch (Exception ex)
            {

                throw new Exception();
            }
        }

        [HttpPost]
        public ActionResult ModifyAppointment(int appointmentId, int actId, int professionnalTitleId, int typeId,
            string startDate, decimal duration, bool isThisOnly, int? wageId)
        {
            //The appointment to modify
            var appointment = _context.ClientAppointments
                .Where(x => x.Id == appointmentId)
                .Single();

            string initialAppointmentDate = appointment.StartDateTime.ToString("dd MMMM");
            string initialAppointmentTime = appointment.StartDateTime.ToString("HH mm");


            //The list of all the therapist appointment
            var appointmentsAlreadyScheduled = _context.ClientAppointments
                .Where(x => x.TherapistId == appointment.TherapistId && !x.IsDeleted)
                .ToList();

            //Get the datetime
            DateTime.TryParseExact(startDate,
                "dd-MM-yyyy HH:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime startDateTime);

            bool didDurationChanged = appointment.NbSession != duration;

            if (appointment.StartDateTime < DateTime.Now || startDateTime < DateTime.Now ||
                (appointment.ClientBillGeneratedDate.HasValue && didDurationChanged))
            {
                throw new Exception();
            }

            //Get the duration in minute of the appointment
            double durationMinute = Decimal.ToDouble(duration * 60);


            //Declare if the therapist is free
            var isTherapistFree = true;
            var conflicts = new List<AppointmentCalendarModel>();

            DateTime initalAppointmentDate = appointment.StartDateTime;

            if (isThisOnly) //Modify only one appointment
            {
                var endDate = startDateTime.AddMinutes(durationMinute);

                #region Verify if the therapist is available

                foreach (var clientAppointment in appointmentsAlreadyScheduled.Where(x => x.Id != appointment.Id))
                {
                    if ((startDateTime >= clientAppointment.StartDateTime && startDateTime < clientAppointment.EndDateTime) ||
                        (endDate > clientAppointment.StartDateTime && endDate <= clientAppointment.EndDateTime) ||
                        (startDateTime < clientAppointment.StartDateTime && endDate > clientAppointment.EndDateTime))
                    {
                        //There is already an appointment
                        isTherapistFree = false;

                        var clientInfos = new GetClientInfos(_context, clientAppointment.ClientId);
                        clientInfos.GetMostRecentClientInfo();

                        //Add it to the conflict list
                        conflicts.Add(new AppointmentCalendarModel
                        {
                            title = $"{clientInfos.FirstName.Decode()} {clientInfos.LastName.Decode()}",
                            start = clientAppointment.StartDateTime.ToString("yyyy-MM-dd HH:mm"),
                            end = clientAppointment.EndDateTime.ToString("yyyy-MM-dd HH:mm")
                        });
                    }
                }


                #endregion

                appointment.NatureActId = actId;
                appointment.ProfessionalTitleId = professionnalTitleId;
                appointment.ConsultationTypeId = typeId;
                appointment.StartDateTime = startDateTime;
                appointment.EndDateTime = endDate;
                appointment.NbSession = duration;
                appointment.TherapistPayInformationId = wageId;
            }
            else //Modify a repetition
            {
                //Declare the difference in hours fromm the original appointment to the modified appointment
                double hourDifference = (startDateTime - appointment.ReferenceStartDateTime.Value).TotalHours;

                var appointmentsToModify = appointmentsAlreadyScheduled
                    .Where(x => x.ClientId == appointment.ClientId
                        && x.RepetitionId == appointment.RepetitionId
                        && x.StartDateTime >= appointment.StartDateTime)
                    .ToList();

                foreach (var oldAppointment in appointmentsToModify)
                {
                    var newStartDateTime = oldAppointment.ReferenceStartDateTime.Value.AddHours(hourDifference);
                    var newEndDateTime = newStartDateTime.AddMinutes(durationMinute);

                    #region Verify if the therapist is available

                    foreach (var clientAppointment in appointmentsAlreadyScheduled.Where(x => x.RepetitionId != appointment.RepetitionId))
                    {
                        if ((newStartDateTime >= clientAppointment.StartDateTime && newStartDateTime < clientAppointment.EndDateTime) ||
                            (newEndDateTime > clientAppointment.StartDateTime && newEndDateTime <= clientAppointment.EndDateTime) ||
                            (newStartDateTime < clientAppointment.StartDateTime && newEndDateTime > clientAppointment.EndDateTime))
                        {
                            //There is already an appointment
                            isTherapistFree = false;

                            var clientInfos = new GetClientInfos(_context, clientAppointment.ClientId);
                            clientInfos.GetMostRecentClientInfo();

                            //Add it to the conflict list
                            conflicts.Add(new AppointmentCalendarModel
                            {
                                title = $"{clientInfos.FirstName.Decode()} {clientInfos.LastName.Decode()}",
                                start = clientAppointment.StartDateTime.ToString("yyyy-MM-dd HH:mm"),
                                end = clientAppointment.EndDateTime.ToString("yyyy-MM-dd HH:mm")
                            });
                        }
                    }


                    #endregion

                    oldAppointment.NatureActId = actId;
                    appointment.ProfessionalTitleId = professionnalTitleId;
                    oldAppointment.ConsultationTypeId = typeId;
                    oldAppointment.StartDateTime = newStartDateTime;
                    oldAppointment.EndDateTime = newEndDateTime;
                    oldAppointment.NbSession = duration;
                    oldAppointment.TherapistPayInformationId = wageId;
                }
            }


            //Block the treatment if there is a conflict
            if (!isTherapistFree)
            {
                return Json(new
                {
                    isSuccess = false,
                    conflicts = conflicts
                });
            }

            try
            {
                _context.SaveChanges();

                if (appointment.ClientBillGeneratedDate.HasValue)
                {
                    #region Doit aviser le client que son rendez-vous a été modifié

                    //Doit aviser le client que son rendez-vous a été modifié
                    var resourceManager = new ResourceManager(typeof(Email));
                    var culture = new CultureInfo(appointment.Clients.LanguageId == 1 ? "fr-CA" : "en-CA");

                    var paymentLink = Url.Action(
                       "Payment",
                       "Client",
                       new { Id = appointment.PaymentUrlCode },
                       protocol: Request.Url.Scheme);


                    string emailSubject = Email.AppointmentModified_Subject;
                    string emailContent = ""; //###TODO mettre le contenu selon les ressource selon si c'est une visio oui présentiel

                    ////////////
                    if (appointment.ConsultationTypeId == 1) //In person
                    {

                        var therapistInfo = new GetTherapistInfo(_context, appointment.TherapistId);
                        therapistInfo.GetMostRecentTherapistInfo();

                        emailContent = string.Format(Email.AppointmentModified_Content_InPerson,

                            initialAppointmentDate,
                            initialAppointmentTime,
                            appointment.StartDateTime.ToString("dd MMMM"),
                            appointment.StartDateTime.ToString("HH:mm"),
                            $"{therapistInfo.Adress}, {therapistInfo.City}",
                            paymentLink);

                    }
                    else if (appointment.ConsultationTypeId == 2) //Online
                    {

                        emailContent = string.Format(Email.AppointmentModified_Content_Online,
                           initialAppointmentDate,
                           initialAppointmentTime,
                           appointment.StartDateTime.ToString("dd MMMM"),
                           appointment.StartDateTime.ToString("HH:mm"),
                           paymentLink);

                    }
                    /////////////





                    var clientInfos = new GetClientInfos(_context, appointment.ClientId);
                    clientInfos.GetMostRecentClientInfo();

                    //Send email
                    EmailHelper.Send(emailSubject,
                        emailContent,
                        clientInfos.Email,
                        "");

                    #endregion
                }

                return Json(new
                {
                    isSuccess = true,
                    firstAppointnment = startDateTime
                });
            }
            catch (Exception ex)
            {
                throw new Exception();
            }


        }

        [HttpPost]
        public ActionResult ModifyAppointmentFromCalendar(int appointmentId, string startDateTimeStr, decimal duration, bool isThisOnly)
        {
            var appointment = _context.ClientAppointments
                .Where(x => x.Id == appointmentId)
                .Single();

            string initialAppointmentDate = appointment.StartDateTime.ToString("dd MMMM");
            string initialAppointmentTime = appointment.StartDateTime.ToString("HH mm");

            //The list of all the therapist appointment
            var appointmentsAlreadyScheduled = _context.ClientAppointments
                .Where(x => x.TherapistId == appointment.TherapistId && !x.IsDeleted)
                .ToList();



            //Get the datetime
            DateTime.TryParseExact(startDateTimeStr,
                "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime startDateTime);

            bool didDurationChanged = appointment.NbSession != duration;

            if (appointment.StartDateTime < DateTime.Now || startDateTime < DateTime.Now ||
                (appointment.ClientBillGeneratedDate.HasValue && didDurationChanged))
            {
                return Json(new
                {
                    isException = true,
                    firstAppointment = startDateTime
                });
            }


            //Get the duration in minute of the appointment
            double durationMinute = Decimal.ToDouble(duration * 60);



            //Declare if the therapist is free
            var isTherapistFree = true;
            var conflicts = new List<AppointmentCalendarModel>();





            if (isThisOnly)
            {
                var endDate = startDateTime.AddMinutes(durationMinute);

                #region Verify if the therapist is available

                foreach (var clientAppointment in appointmentsAlreadyScheduled.Where(x => x.Id != appointment.Id))
                {
                    if ((startDateTime >= clientAppointment.StartDateTime && startDateTime < clientAppointment.EndDateTime) ||
                        (endDate > clientAppointment.StartDateTime && endDate <= clientAppointment.EndDateTime) ||
                        (startDateTime < clientAppointment.StartDateTime && endDate > clientAppointment.EndDateTime))
                    {
                        //There is already an appointment
                        isTherapistFree = false;

                        var clientInfos = new GetClientInfos(_context, clientAppointment.ClientId);
                        clientInfos.GetMostRecentClientInfo();

                        //Add it to the conflict list
                        conflicts.Add(new AppointmentCalendarModel
                        {
                            title = $"{clientInfos.FirstName.Decode()} {clientInfos.LastName.Decode()}",
                            start = clientAppointment.StartDateTime.ToString("yyyy-MM-dd HH:mm"),
                            end = clientAppointment.EndDateTime.ToString("yyyy-MM-dd HH:mm")
                        });
                    }
                }


                #endregion

                appointment.StartDateTime = startDateTime;
                appointment.EndDateTime = endDate;
                appointment.NbSession = duration;
            }
            else
            {
                //Declare the difference in hours fromm the original appointment to the modified appointment
                double hourDifference = (startDateTime - appointment.ReferenceStartDateTime.Value).TotalHours;

                var appointmentsToModify = appointmentsAlreadyScheduled
                    .Where(x => x.ClientId == appointment.ClientId
                        && x.RepetitionId == appointment.RepetitionId
                        && x.StartDateTime >= appointment.StartDateTime)
                    .ToList();

                //We cant change the duration and the startTime of the appointment simultaniously
                //so if the appointment start date is the same as the startDateTime specified
                //then we know that we are mofifying the duration
                var isModifyDuration = appointment.StartDateTime == startDateTime;

                foreach (var oldAppointment in appointmentsToModify)
                {
                    //If we are modyfing the start datetie of the appointment, we dont want to modify
                    //the duration
                    if (!isModifyDuration)
                    {
                        duration = oldAppointment.NbSession;
                        durationMinute = Decimal.ToDouble(oldAppointment.NbSession * 60);
                    }

                    var newStartDateTime = oldAppointment.ReferenceStartDateTime.Value.AddHours(hourDifference);
                    var newEndDateTime = newStartDateTime.AddMinutes(durationMinute);

                    #region Verify if the therapist is available

                    foreach (var clientAppointment in appointmentsAlreadyScheduled.Where(x => x.RepetitionId != appointment.RepetitionId))
                    {
                        if ((newStartDateTime >= clientAppointment.StartDateTime && newStartDateTime < clientAppointment.EndDateTime) ||
                            (newEndDateTime > clientAppointment.StartDateTime && newEndDateTime <= clientAppointment.EndDateTime) ||
                            (newStartDateTime < clientAppointment.StartDateTime && newEndDateTime > clientAppointment.EndDateTime))
                        {
                            //There is already an appointment
                            isTherapistFree = false;

                            var clientInfos = new GetClientInfos(_context, clientAppointment.ClientId);
                            clientInfos.GetMostRecentClientInfo();

                            //Add it to the conflict list
                            conflicts.Add(new AppointmentCalendarModel
                            {
                                title = $"{clientInfos.FirstName.Decode()} {clientInfos.LastName.Decode()}",
                                start = clientAppointment.StartDateTime.ToString("yyyy-MM-dd HH:mm"),
                                end = clientAppointment.EndDateTime.ToString("yyyy-MM-dd HH:mm")
                            });
                        }
                    }

                    #endregion

                    oldAppointment.StartDateTime = newStartDateTime;
                    oldAppointment.EndDateTime = newEndDateTime;
                    oldAppointment.NbSession = duration;
                }

            }

            //Block the treatment if there is a conflict
            if (!isTherapistFree)
            {
                return Json(new
                {
                    isSuccess = false,
                    conflicts = conflicts,
                    firstAppointment = startDateTime
                });
            }

            try
            {
                _context.SaveChanges();

                if (appointment.ClientBillGeneratedDate.HasValue)
                {
                    #region Doit aviser le client que son rendez-vous a été modifié

                    //Doit aviser le client que son rendez-vous a été modifié
                    var resourceManager = new ResourceManager(typeof(Email));
                    var culture = new CultureInfo(appointment.Clients.LanguageId == 1 ? "fr-CA" : "en-CA");

                    var paymentLink = Url.Action(
                       "Payment",
                       "Client",
                       new { Id = appointment.PaymentUrlCode },
                       protocol: Request.Url.Scheme);


                    string emailSubject = Email.AppointmentModified_Subject;
                    string emailContent = ""; //###TODO mettre le contenu selon les ressource selon si c'est une visio oui présentiel

                    ////////////
                    if (appointment.ConsultationTypeId == 1) //In person
                    {

                        var therapistInfo = new GetTherapistInfo(_context, appointment.TherapistId);
                        therapistInfo.GetMostRecentTherapistInfo();

                        emailContent = string.Format(Email.AppointmentModified_Content_InPerson,

                           initialAppointmentDate,
                            initialAppointmentTime,
                            appointment.StartDateTime.ToString("dd MMMM"),
                            appointment.StartDateTime.ToString("HH:mm"),
                            $"{therapistInfo.Adress}, {therapistInfo.City}",
                            paymentLink);

                    }
                    else if (appointment.ConsultationTypeId == 2) //Online
                    {

                        emailContent = string.Format(Email.AppointmentModified_Content_Online,
                           initialAppointmentDate,
                           initialAppointmentTime,
                           appointment.StartDateTime.ToString("dd MMMM"),
                           appointment.StartDateTime.ToString("HH:mm"),
                           paymentLink);

                    }
                    /////////////



                    var clientInfos = new GetClientInfos(_context, appointment.ClientId);
                    clientInfos.GetMostRecentClientInfo();

                    //Send email
                    EmailHelper.Send(emailSubject,
                        emailContent,
                        clientInfos.Email,
                        "");

                    #endregion
                }

                return Json(new
                {
                    isSuccess = true,
                    firstAppointment = startDateTime
                });
            }
            catch (Exception ex)
            {
                throw new Exception();
            }


        }

        [HttpPost]
        public ActionResult DeleteAppointment(int appointmentId, bool isThisOnly, int deleteReasonId)
        {
            var appointment = _context.ClientAppointments
                .Where(x => x.Id == appointmentId)
                .Single();

            //Cant modify an old appointment. Will show the error modal
            if (appointment.StartDateTime < DateTime.Now)
                throw new Exception();

            if (isThisOnly)
            {
                appointment.IsDeleted = true;
                appointment.ReasonDeleteClientAppointmentId = deleteReasonId;

                //Remove the appointmentUrl so no paiement can be made
                appointment.PaymentUrlCode = null;
                var refundClientPrePayment = new RefundClientPrePayment(appointment, _context);
            }
            else
            {
                var appointments = _context.ClientAppointments
                    .Where(x => x.RepetitionId == appointment.RepetitionId);

                foreach (var app in appointments)
                {
                    app.IsDeleted = true;
                    app.ReasonDeleteClientAppointmentId = deleteReasonId;

                    //Remove the appointmentUrl so no paiement can be made
                    app.PaymentUrlCode = null;
                    var refundClientPrePayment = new RefundClientPrePayment(app, _context);
                }
            }

            try
            {
                _context.SaveChanges();
                return Json(new { success = "success" });
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        [HttpPost]
        public ActionResult ReturnClientToAdministration(int clientId, string message)
        {
            #region Verification

            //The message field cant be empty
            if (string.IsNullOrWhiteSpace(message))
                throw new Exception();


            //Cant return the client if there is already an appointment booked
            if (_context.ClientAppointments.Where(x => x.ClientId == clientId).Any())
                throw new Exception();

            //On ne peut pas retourner un client externe
            var client = _context.Clients
                .Where(x => x.Id == clientId)
                .Single();

            if (client.IsExternalClient)
                throw new Exception();

            #endregion

            //Get the last occurence by using orderbydescending first
            var therapistClientRequest = _context.TherapistClientRequest
                .Where(x => x.ClientId == clientId && x.TherapistId == _therapistObject.Id)
                .OrderByDescending(x => x.RequestSentDate)
                .First();

            therapistClientRequest.IsAccepted = false;
            therapistClientRequest.AdditionMessageFromTherapist = message;

            client.TherapistId = null;

            try
            {
                _context.SaveChanges();
                return Json(new { success = "success" });
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        #endregion

        #region Confirm presence

        public ActionResult ConfirmPresence()
        {
            var viewModel = new ConfirmPresenceViewModel(_context, _therapistObject.Id);
            return View(viewModel);
        }


        public ActionResult ConfirmPresenceChangeStatus(int appointmentId, int statusId)
        {

            var appointment = _context.ClientAppointments
                .Where(x => x.Id == appointmentId)
                .Single();

            appointment.AppointmentStatusId = statusId;

            //Email infos
            string emailSubject = "";
            string emailContent = "";

            if (statusId == 4 || statusId == 5) // Canceled with charges or completed
            {

                var resourceManager = new ResourceManager(typeof(Email));
                var culture = new CultureInfo(appointment.Clients.LanguageId == 1 ? "fr-CA" : "en-CA");

                var paymentLink = Url.Action(
                    "Payment",
                    "Client",
                    new { Id = appointment.PaymentUrlCode },
                    protocol: Request.Url.Scheme);

                if (appointment.ClientPaymentDate == null)
                {
                    //Resend payment link to the client
                    emailSubject = resourceManager.GetString("AppointmentNeedPayment_Subject", culture);
                    emailContent = string.Format(resourceManager.GetString("AppointmentNeedPayment_Content", culture),
                        $"<a href=\"{paymentLink}\">{paymentLink}</a>");
                }
                else if (appointment.ClientPaymentDate < appointment.StartDateTime)
                {
                    //It was a prepaiement, proccess the prepaiement and send an email to the client for the receipt

                    #region Verification

                    var appointmentPayment = appointment.ClientPaymentInfos
                        .OrderByDescending(x => x.TransactionDateTime)
                        .First();

                    //There is no prepayment in the database
                    if (appointmentPayment == null)
                        throw new Exception();

                    #endregion

                    #region Get the appointment informations

                    var appointmentHelper = new AppointmentHelper(appointmentId, _context);
                    decimal totalCost = appointmentHelper.GetAppointmentCost();
                    string uniqueRefPrePayment = appointmentHelper.GetPrePaymentUniqueRef();

                    #endregion

                    #region Process the prepayment

                    var nuveiPayment = new NuveiPreAuthCompletion(totalCost, uniqueRefPrePayment, DateTime.Now, _context);
                    nuveiPayment.UrlPayment(appointment);

                    #endregion

                    #region Email section

                    //Inform the client that he can now download his receipt
                    emailSubject = resourceManager.GetString("ReceiptReady_Subject", culture);
                    emailContent = string.Format(resourceManager.GetString("ReceiptReady_Content", culture),
                        $"<a href=\"{paymentLink}\">{paymentLink}</a>");

                    #endregion

                }
                else
                {
                    //Appointment is paid, send a link to the client for the downlaod receipt
                    emailSubject = resourceManager.GetString("ReceiptReady_Subject", culture);
                    emailContent = string.Format(resourceManager.GetString("ReceiptReady_Content", culture),
                        $"<a href=\"{paymentLink}\">{paymentLink}</a>");
                }

                var clientInfos = new GetClientInfos(_context, appointment.ClientId);
                clientInfos.GetMostRecentClientInfo();

                //Send email
                EmailHelper.Send(emailSubject,
                        emailContent,
                        clientInfos.Email,
                        "");
            }
            else
            {
                //Remove the appointmentUrl so no paiement can be made
                appointment.PaymentUrlCode = null;

                var refundClientPrePayment = new RefundClientPrePayment(appointment, _context);
            }

            try
            {
                _context.SaveChanges();
                return Json(new { success = "success" });
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        #endregion

        #region Client Not Payed

        public ActionResult AppointmentsNotPayed()
        {
            var viewModel = new AppointmentNotPayedViewModel(_context, _therapistObject.Id);
            return View(viewModel);
        }


        public ActionResult DeleteNotPayedAppointment(int appointmentId)
        {
            var appointment = _context.ClientAppointments
                .Where(x => x.Id == appointmentId)
                .Single();

            appointment.IsDeleted = true;
            appointment.ReasonDeleteClientAppointmentId = 5; //Client didn't pay 

            try
            {
                _context.SaveChanges();
                return Json(new { success = "success" });
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }



        public ActionResult SendBackPaymentLinkToClient(int appointmentId, string clientEmail, string message)
        {
            var appointment = _context.ClientAppointments
                .Where(x => x.Id == appointmentId)
                .Single();

            var paymentLink = Url.Action(
                    "Payment",
                    "Client",
                    new { Id = appointment.PaymentUrlCode },
                    protocol: Request.Url.Scheme);

            var resourceManager = new ResourceManager(typeof(Email));
            var culture = new CultureInfo(appointment.Clients.LanguageId == 1 ? "fr-CA" : "en-CA");

            var dollarAmount = appointment.TotalDollarAmount;
            var emailSubject = resourceManager.GetString("AppointmentNeedPayment_Manual_Subject", culture);
            var emailContent = String.Format(resourceManager.GetString("AppointmentNeedPayment_Manuel_Content", culture),
                appointment.StartDateTime.ToString("dd MMM yyyy"),
                $"<a href=\"{paymentLink}\">{paymentLink}</a>",
                message
                );

            try
            {
                EmailHelper.Send(emailSubject, emailContent, clientEmail, "");

                return Json(new { success = "success" });
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        #endregion

        #region Claim form


        public ActionResult ClaimForm()
        {
            var viewModel = new ClaimFormViewModel(_context, _therapistObject);
            return View(viewModel);
        }

        public ActionResult ClaimAppointments(string appointmentIdsStr)
        {
            var appointmentIds = appointmentIdsStr.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            var appointments = new List<ClientAppointments>();

            //Add the appointment to the list
            var allAppointments = _context.ClientAppointments.ToList();

            foreach (var id in appointmentIds)
            {
                var appointment = allAppointments
                    .Where(x => x.Id == id)
                    .Single();

                appointments.Add(appointment);
            }

            #region Verification

            var isLegit = true;
            //Check if all the appointment claimed are claimable and belong to the said therapist
            foreach (var appointment in appointments)
            {
                if (appointment.TherapistId != _therapistObject.Id || appointment.TherapistBillId != null ||
                    appointment.ClientPaymentDate == null)
                    isLegit = false;
            }

            if (!isLegit)
            {
                //Something is off, so threw an exception to not continue the process
                throw new Exception();
            }

            #endregion

            //Get the last bill Id
            var lastTherapistBillId = _context.ClientAppointments
                .Where(x => x.TherapistId == _therapistObject.Id)
                .OrderByDescending(x => x.TherapistBillId)
                .Select(x => x.TherapistBillId)
                .FirstOrDefault();

            var therapistBillId = lastTherapistBillId == null ? 1 : lastTherapistBillId.Value + 1;

            var therapistBill = new TherapistBill();
            therapistBill.TherapistId = _therapistObject.Id;
            therapistBill.BillNumber = therapistBillId;
            therapistBill.Date = DateTime.Now;

            #region Get the amount of money

            var totalBillDollarAmount = 0m;
            var totalTps = 0m;
            var totalTvq = 0m;

            foreach (var appointment in appointments)
            {
                var getClaimableAmountForAppointment = new GetClaimableAmountForAppointment(_context);
                getClaimableAmountForAppointment.CalculateAppointmentPayment(appointment.Id);
                totalBillDollarAmount += getClaimableAmountForAppointment.ClaimableAmount;
                totalTps += getClaimableAmountForAppointment.TpsAmount;
                totalTvq += getClaimableAmountForAppointment.TvqAmount;
            }

            #endregion

            therapistBill.SubTotal = totalBillDollarAmount;
            therapistBill.TpsAmount = totalTps;
            therapistBill.TvqAmount = totalTvq;


            _context.TherapistBill.Add(therapistBill);

            #region Set the bill id to the appointment

            foreach (var appointment in appointments)
            {
                appointment.TherapistBillId = therapistBillId;
            }

            #endregion

            try
            {
                _context.SaveChanges();
                return Json(new { success = "success" });
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }


        public ActionResult ApplyFilterTableBillSent(string startDateStr, string endDateStr, int pageNumber)
        {
            DateTime? startDateTime = null;
            DateTime? endDateTime = null;


            if (!string.IsNullOrWhiteSpace(startDateStr))
                startDateTime = DateTime.ParseExact(startDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            if (!string.IsNullOrWhiteSpace(endDateStr))
                endDateTime = DateTime.ParseExact(endDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            if (endDateTime != null)
                endDateTime = endDateTime.Value.AddDays(1).AddSeconds(-1);


            var billsSent = _context.GetListTherapistBillSent(
                _therapistObject.Id,
                startDateTime,
                endDateTime,
                pageNumber,
                WebSiteProperties.NbResultPerPage)
                .ToList();

            //Format payment status
            var resourceManager = new ResourceManager(typeof(Resource));
            foreach (var bill in billsSent)
                bill.PaymentStatus = resourceManager.GetString(bill.PaymentStatus);

            var count = _context.GetListTherapistBillSentCount(
                _therapistObject.Id,
                startDateTime,
                endDateTime).ToList();

            var nbPage =
                Convert.ToInt32(
                    Math.Ceiling(
                        Convert.ToDecimal(count[0])
                        /
                        Convert.ToDecimal(WebSiteProperties.NbResultPerPage)
                    )
                );


            return Json
                (
                    new
                    {
                        bills = billsSent,
                        nbPage = nbPage
                    }
                );
        }


        public ActionResult DownloadExcelTherapistBillsSent(string startDateStr, string endDateStr)
        {

            DateTime? startDateTime = null;
            DateTime? endDateTime = null;


            if (!string.IsNullOrWhiteSpace(startDateStr))
                startDateTime = DateTime.ParseExact(startDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            if (!string.IsNullOrWhiteSpace(endDateStr))
                endDateTime = DateTime.ParseExact(endDateStr, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            if (endDateTime != null)
                endDateTime = endDateTime.Value.AddDays(1).AddSeconds(-1);

            var bills = _context.GetListTherapistBillSent(
                _therapistObject.Id,
                startDateTime,
                endDateTime,
                1,
                9_999_999)
                .ToList();

            var resourceManager = new ResourceManager(typeof(Resource));

            var culture = CultureInfo.CurrentCulture.Name;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excel = new ExcelPackage())
            {
                // Crée la feuille de travail "Liste de prix"
                string excelSheetName = Resource.ClaimForm_ExcelDocument_TabName;
                excel.Workbook.Worksheets.Add(excelSheetName);

                // Insère l'information sur les paramêtres utiliser pour générer la liste de prix
                List<string[]> lstExcellRow = new List<string[]>();
                int rowStartAt = 2;
                lstExcellRow.Add(new string[] {
                    Resource.ClaimForm_ExcelDocumentTableHeader_DateSent,
                    Resource.ClaimForm_ExcelDocumentTableHeader_BillNumber,
                    Resource.ClaimForm_ExcelDocumentTableHeader_PaymentStatus,
                    Resource.ClaimForm_ExcelDocumentTableHeader_Amount,
                    Resource.ClaimForm_ExcelDocumentTableHeader_Tps,
                    Resource.ClaimForm_ExcelDocumentTableHeader_Tvq
                });

                // Determine les colonnes où insérer l'info (e.g. A1:D1)
                string rowRange = "B" + rowStartAt + ":" + Char.ConvertFromUtf32(lstExcellRow[0].Length + 64) + rowStartAt++;

                // Récupère la feuille de travail
                ExcelWorksheet worksheet = excel.Workbook.Worksheets[excelSheetName];

                // Insère la ligne dans la feuille de travail
                worksheet.Cells[rowRange].LoadFromArrays(lstExcellRow);

                bills.ForEach(x =>
                {
                    lstExcellRow.Add(new string[] {
                        x.DateSend.ToString("yyyy-MM-dd"),
                        "RSPYF-" + x.BillNumber,
                        resourceManager.GetString(x.PaymentStatus),
                        x.Subtotal.ToString(),
                        x.TpsAmount.ToString(),
                        x.TvqAmount.ToString()
                    });
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

                string excelName = DateTime.Today.ToString("yyyy_MM_dd") + "Facture_Therapeute";

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

        [HttpPost]
        public ActionResult DownloadTherapistBillPdf(int billNumber)
        {
            string pageHtmlPathUrl = Server.MapPath("/PDF/TherapistBillTemplate.html");
            string[] pageContentHtml = System.IO.File.ReadAllLines(pageHtmlPathUrl);

            var therapistBill = _context.TherapistBill
                .Where(x => x.TherapistId == _therapistObject.Id && x.BillNumber == billNumber)
                .First();

            //Get the list of appointment from that bill
            var clientAppointments = _context.ClientAppointments
                .Where(x => x.TherapistId == _therapistObject.Id && x.TherapistBillId == billNumber)
                .ToList();

            var generateTherapistBill = new GenerateTherapistBillPdf(pageContentHtml, clientAppointments, _context, therapistBill);

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=RSPYF-" + billNumber.ToString() + ".pdf");
            Response.BinaryWrite(generateTherapistBill.PdfBuffer);
            Response.End();


            return Json(new { success = "success" });

        }

        #endregion

        #region DashBoard

        public ActionResult Dashboard()
        {
            var viewModel = new DashboardTherapistViewModel(_context, _therapistObject);

            return View(viewModel);
        }

        #endregion

        #region Client List

        public ActionResult ClientList()
        {
            var viewModel = new ClientListTherapistViewModel(_context, _therapistObject);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ClientListApplyFilter(int pageNumber, bool? isExternalClient, bool isActive)
        {
            List<GetClientListForTherapistClientListPage_Result> clients = _context.GetClientListForTherapistClientListPage(
                pageNumber,
                WebSiteProperties.NbResultPerPage,
                _therapistObject.Id,
                isActive,
                isExternalClient)
                .ToList();

            var nbResult = _context.GetClientListForTherapistClientListPageCount(
                _therapistObject.Id,
                isActive,
                isExternalClient)
                .First();

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

        [HttpPost]
        public ActionResult DeleteClient(int clientId)
        {

            var client = _context.Clients
                .Where(x => x.Id == clientId)
                .Single();

            client.IsDeleted = true;

            try
            {
                _context.SaveChanges();
                return Json(new { msg = "Success" });
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        [HttpPost]
        public ActionResult RestoreClient(int clientId)
        {
            var client = _context.Clients
                .Where(x => x.Id == clientId)
                .Single();

            client.IsDeleted = false;

            try
            {
                _context.SaveChanges();
                return Json(new { msg = "Success" });
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        #endregion

        #region Detail Client

        public ActionResult DetailClient(int? clientId)
        {
            DetailClientTherapistViewModel viewModel;
            if (clientId != null)
            {
                viewModel = new DetailClientTherapistViewModel(_context, clientId.Value, _therapistObject);
            }
            else
            {
                viewModel = new DetailClientTherapistViewModel(_context);
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
                datas.Email.Trim() == "" ||
                !datas.ClientHourlyCost.HasValue ||
                datas.TherapistHourlyWage.HasValue)
            {
                throw new Exception();
            }

            #endregion

            var createClient = new CreateClientForTherapist(_context, datas, _therapistObject);
            if (!createClient.Succeeded)
            {
                //La création du thérapeute a échoué, on retourne le form
                throw new Exception();
            }

            TempData["Message"] = Resource.DetailClientAdmin_SucceededCreated;

            return Json(new { result = "Redirect", url = Url.Action("ClientList", "therapist") });

        }

        public ActionResult EditClient(DetailClientDatas datas)
        {
            #region Verify the datas

            if (datas.FirstName.Trim() == "" ||
                datas.LastName.Trim() == "" ||
                datas.Email.Trim() == "" ||
                datas.TherapistHourlyWage.HasValue)
            {
                throw new Exception();
            }


            var client = _context.Clients
                .Where(x => x.Id == datas.ClientId)
                .Single();

            if ((client.IsExternalClient && datas.ClientHourlyCost == null) ||
                (!client.IsExternalClient && datas.ClientHourlyCost != null))
                throw new Exception();

            #endregion

            var editClient = new EditClientForTherapist(_context, datas, datas.ClientId.Value);
            if (!editClient.Succeeded)
            {
                //La modification du thérapeute a échoué, on renvoie le form
                throw new Exception();
            }

            return Json(new { success = true });
        }


        #endregion

        #region Client receipt

        public ActionResult ClientReceipt()
        {
            var viewModel = new ListClientReceiptsViewModel(_context, _therapistObject);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ClientReceiptApplyFilter(DateTime? startDate, DateTime? endDate, string clientName, int pageNumber)
        {
            if (endDate.HasValue)
                endDate = endDate.Value.AddDays(1).AddSeconds(-1);

            var clientReceipt = _context.GetListClientReceipt(
                startDate,
                endDate,
                clientName.ToLower(),
                _therapistObject.Id,
                pageNumber,
                _nbResultPerPage)
                .ToList();

            var count = _context.GetListClientReceiptCount(
                startDate,
                endDate,
                clientName.ToLower(),
                _therapistObject.Id)
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
        public ActionResult DownloadExcelClientReceipt(DateTime? startDate, DateTime? endDate, string clientName)
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
                _therapistObject.Id,
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

    }
}