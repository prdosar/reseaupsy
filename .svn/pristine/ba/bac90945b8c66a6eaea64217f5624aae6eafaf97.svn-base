using Business;
using Microsoft.AspNet.Identity;
using ReseauPsy.Models;
using ReseauPsy.Models.Therapist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ReseauPsy.Controllers.Therapist.Api
{
    [Authorize(Roles = RoleName.Therapist)]
    public class AppointmentModalClientInfosController : ApiController
    {
        #region Database context

        private ReseauPsyEntities _context;

        public AppointmentModalClientInfosController()
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

        //GET /api/AppointmentModalClientInfos?clientid=XX&isFrench=
        public AppointmentModalModel GetClientInfos(int clientId, bool isFrench)
        {
            return new AppointmentModalModel(_context, clientId, isFrench);
        }

    }

    //[Authorize(Roles = RoleName.Therapist)]
    public class AppointmentModalAppointmentInfosController : ApiController
    {
        #region Database context

        private ReseauPsyEntities _context;

        public AppointmentModalAppointmentInfosController()
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

        //GET /api/AppointmentModalAppointmentInfos?appointmentid=&isFrench=
        public AppointmentModalAppointmentInfos GetAppointmentInfo(int appointmentId, bool isFrench)
        {
            return new AppointmentModalAppointmentInfos(_context, appointmentId, isFrench);
        }
    }
}
