using Business;
using Library.Helper;
using ReseauPsy.Models;
using ReseauPsy.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ReseauPsy.Controllers.Admin.Api
{
    [Authorize(Roles = RoleName.AdminFullAccess)]
    public class GetTherapistController : ApiController
    {
        #region Database context

        private ReseauPsyEntities _context;

        public GetTherapistController()
        {
            _context = new ReseauPsyEntities();
        }

        #endregion

        //GET /api/GetTherapist
        public IEnumerable<CalendarResource> GetTherapists()
        {


            var therapists =  _context.Therapists
                .Where(x => !x.IsDeleted)
                .Select(x => new CalendarResource
                {
                    id = x.Id.ToString(),
                    title = x.TherapistInfo
                        .OrderByDescending(y => y.ModificationDate)
                        .Select(y => y.FirstName)
                        .FirstOrDefault() + " " + 
                        x.TherapistInfo
                        .OrderByDescending(y => y.ModificationDate)
                        .Select(y => y.LastName)
                        .FirstOrDefault()
                }).ToList();

            foreach (var therapist in therapists)
            {
                therapist.title = therapist.title.Decode();
            }

            return therapists;
        }
    }

    [Authorize(Roles = RoleName.AdminFullAccess)]
    public class GetTherapistAppointmentController : ApiController
    {
        #region Database context

        private ReseauPsyEntities _context;

        public GetTherapistAppointmentController()
        {
            _context = new ReseauPsyEntities();
        }

        #endregion

        //GET /api/GetTherapistAppointment?start=2021-10-10&end=2021-10-16
        public IEnumerable<CalendarTherapistAppointment> GetAppointments(string start, string end)
        {
            var startDate = DateTime.Parse(start, null, System.Globalization.DateTimeStyles.RoundtripKind);
            var endDate = DateTime.Parse(end, null, System.Globalization.DateTimeStyles.RoundtripKind);

            var appointments = new List<CalendarTherapistAppointment>();

            var clientAppointments = _context.ClientAppointments
                .Where(x => !x.IsDeleted &&
                    x.StartDateTime >= startDate &&
                    x.EndDateTime <= endDate);

            foreach (var appointment in clientAppointments)
            {
                var clientInfos = new GetClientInfos(_context, appointment.ClientId);
                clientInfos.GetOldClientInfo(appointment.StartDateTime);

                appointments.Add(new CalendarTherapistAppointment
                {
                    
                    id = appointment.Id.ToString(),
                    resourceId = appointment.TherapistId.ToString(),
                    title = $"{clientInfos.FirstName.Decode()} {clientInfos.LastName.Decode()}",
                    start = appointment.StartDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    end = appointment.EndDateTime.ToString("yyyy-MM-ddTHH:mm:ss")
                });
            }

            return appointments;
        }

    }
}
