using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Business;
using Library.Helper;
using ReseauPsy.Models;
using ReseauPsy.Models.Admin;

namespace ReseauPsy.ViewModel.Admin
{
    public class ListTherapistBillViewModel
    {
        public List<GetListTherapistBillForAdmin_Result> TherapistBills { get; set; }
        public int TherapistBillCount { get; set; }
        public List<DbTableIdNameProperties> Therapists { get; set; }
        public int NbPage { get; set; }
        public int NbPagerPageShown { get; set; }

        public ListTherapistBillViewModel(ReseauPsyEntities _context)
        {
            var therapistHelper = new TherapistHelper(_context);
            this.Therapists = therapistHelper.GetListTherapistIdName();

            this.NbPagerPageShown = WebSiteProperties.NbPagerShown;

            this.TherapistBills = _context.GetListTherapistBillForAdmin(
                null,
                null,
                null,
                null,
                1,
                WebSiteProperties.NbResultPerPage)
                .ToList();

            foreach (var bill in this.TherapistBills)
            {
                bill.TherapistName = bill.TherapistName.Decode();
            }

            this.TherapistBillCount = Convert.ToInt32(_context.GetListTherapistBillForAdminCount(
                null,
                null,
                null,
                null).ToList()[0]);

            this.NbPage =
                Convert.ToInt32(
                    Math.Ceiling(
                        Convert.ToDecimal(this.TherapistBillCount)
                        /
                        Convert.ToDecimal(WebSiteProperties.NbResultPerPage)
                    )
                );
        }
    }
}