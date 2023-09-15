using Business;
using Library.Helper;
using ReseauPsy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.ViewModel.Therapist
{
    public class ClientListTherapistViewModel
    {
        public List<GetClientListForTherapistClientListPage_Result> Clients { get; set; }
        public int NbPage { get; set; }
        public int NbPagerPageShown { get; set; }

        public ClientListTherapistViewModel(ReseauPsyEntities context, Business.Therapist therapist)
        {
            Clients = context.GetClientListForTherapistClientListPage(
                1,
                WebSiteProperties.NbResultPerPage,
                therapist.Id,
                true,
                null)
                .ToList();

            foreach (var client in Clients)
            {
                client.ClientName = client.ClientName.Decode();
            }

            this.NbPagerPageShown = WebSiteProperties.NbPagerShown;

            int clientCount = Convert.ToInt32(context.GetClientListForTherapistClientListPageCount(
                therapist.Id,
                true,
                null)
                .First());

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