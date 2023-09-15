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
    public class ClientListViewModel
    {
        public List<GetClientListForAdminClientListPage_Result> Clients { get; set; }
        public List<DbTableIdNameProperties> Therapists { get; set; }
        public int NbPage { get; set; }
        public int NbPagerPageShown { get; set; }

        public ClientListViewModel(ReseauPsyEntities context)
        {
            var therapistHelper = new TherapistHelper(context);
            this.Therapists = therapistHelper.GetListTherapistIdName();

            Clients = context.GetClientListForAdminClientListPage(
                null,
                1,
                WebSiteProperties.NbResultPerPage)
                .ToList();

            foreach (var client in Clients)
            {
                client.ClientName = client.ClientName.Decode();
                client.TherapistName = client.TherapistName.Decode();
            }

            this.NbPagerPageShown = WebSiteProperties.NbPagerShown;

            int clientCount = Convert.ToInt32(context.GetClientListForAdminClientListPageCount(null).First());

            this.NbPage =
                Convert.ToInt32(
                    Math.Ceiling(
                        Convert.ToDecimal(clientCount)
                        /
                        Convert.ToDecimal(WebSiteProperties.NbResultPerPage)
                    )
                );
        }
    }
}