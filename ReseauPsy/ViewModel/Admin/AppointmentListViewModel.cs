using Business;
using Library.Helper;
using ReseauPsy.Models;
using ReseauPsy.Models.Admin;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ReseauPsy.ViewModel.Admin
{
    public class AppointmentListViewModel
    {
        public List<DbTableIdNameProperties> AppointmentStatus { get; set; }
        public List<DbTableIdNameProperties> Therapists { get; set; }
        public List<GetListAppointmentForAdmin_Result> Appointments { get; set; }
        public List<GetListAppointmentForAdminCount_Result> AppointmentCount { get; set; }
        public int NbPage { get; set; }
        public int NbPagerPageShown { get; set; }



        public AppointmentListViewModel(ReseauPsyEntities _context)
        {
            var isFrench = CultureInfo.CurrentCulture.Name == "fr-CA";
            this.NbPagerPageShown = WebSiteProperties.NbPagerShown;


            this.AppointmentStatus = _context.AppointmentStatus
                .Select(x => new DbTableIdNameProperties
                {
                    Id = x.Id,
                    Name = isFrench ? x.Status : x.StatusEN
                })
                .ToList();


            var therapistHelper = new TherapistHelper(_context);
            this.Therapists = therapistHelper.GetListTherapistIdName();

            this.Appointments = _context.GetListAppointmentForAdmin(
                isFrench,
                null,
                null,
                null,
                null,
                null,
                1,
                WebSiteProperties.NbResultPerPage)
                .ToList();

            foreach (var appointment in this.Appointments)
            {
                appointment.TherapistName = appointment.TherapistName.Decode();
                appointment.ClientName = appointment.ClientName.Decode();
            }

            this.AppointmentCount = _context.GetListAppointmentForAdminCount(
                null,
                null,
                null,
                null,
                null)
                .ToList();

            this.NbPage =
                Convert.ToInt32(
                    Math.Ceiling(
                        Convert.ToDecimal(AppointmentCount[0].TotalResult)
                        /
                        Convert.ToDecimal(WebSiteProperties.NbResultPerPage)
                    )
                );
        }
    }
}