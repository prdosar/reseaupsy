using Business;
using Library.Helper;
using Microsoft.AspNet.Identity;
using ReseauPsy.Models;
using ReseauPsy.Models.Therapist;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ReseauPsy.Controllers.Therapist.Api
{
    [Authorize(Roles = RoleName.Therapist)]
    public class AppointmentCalendarController : ApiController
    {
        #region Database context

        private ReseauPsyEntities _context;

        public AppointmentCalendarController()
        {
            _context = new ReseauPsyEntities();
        }

        #endregion

        #region Therapist

        public string TherapistIdentityId
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }

        public Business.Therapist TherapistObject
        {
            get
            {
                return _context.Therapists
                    .Where(x => x.AspNetUserId == TherapistIdentityId)
                    .First();
            }
        }


        #endregion

        //GET /api/AppointmentCalendar?start=2021-10-10&end=2021-10-16
        public IEnumerable<AppointmentCalendarModel> GetAppointments(string start, string end)
        {
            var isFrench = CultureInfo.CurrentCulture.Name == "fr-CA";
            var startDate = DateTime.Parse(start, null, System.Globalization.DateTimeStyles.RoundtripKind);
            var endDate = DateTime.Parse(end, null, System.Globalization.DateTimeStyles.RoundtripKind);

            var appointments = new List<AppointmentCalendarModel>();

            var clientAppointments = _context.ClientAppointments
                .Where(x => x.TherapistId == TherapistObject.Id &&
                    !x.IsDeleted &&
                    x.StartDateTime >= startDate &&
                    x.EndDateTime <= endDate)
                .ToList();


            foreach (var appointment in clientAppointments)
            {
                var clientInfos = new GetClientInfos(_context, appointment.ClientId);
                clientInfos.GetMostRecentClientInfo();

                appointments.Add(new AppointmentCalendarModel
                {
                    title = $"{clientInfos.FirstName.Decode()} {clientInfos.LastName.Decode()}",
                    start = appointment.StartDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    end = appointment.EndDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    editable = appointment.StartDateTime > DateTime.Now,

                    extendedProps = new AppointmentCalendarExtendedProposModel
                    {
                        ClientId = appointment.ClientId,
                        AppointmentId = appointment.Id,
                        RepetitionId = appointment.RepetitionId,
                        IsEditable = appointment.StartDateTime > DateTime.Now,
                        IsDurationEditable = !appointment.ClientBillGeneratedDate.HasValue,
                    }
                });
            }

            return appointments;
        }
    }
}
