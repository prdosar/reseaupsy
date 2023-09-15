using Business;
using Library.Helper;
using ReseauPsy.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ReseauPsy.ViewModel.Admin
{
    public class ExternnalAssociationListViewModel
    {
        public List<DbTableIdNameProperties> Status { get; set; }
        public List<ExternalAssociation> ExternalAssociations { get; set; }
        public List<GetClientSentToExternalAssociation_Result> Clients { get; set; }
        public int CountNotDefined { get; set; }
        public int CountTookInCharge { get; set; }
        public int CountNotTakeInCharge { get; set; }
        public int NbPage { get; set; }
        public int NbPagerPageShown { get; set; }


        public ExternnalAssociationListViewModel(ReseauPsyEntities _context)
        {
            this.ExternalAssociations = _context.ExternalAssociations.ToList();
            this.NbPagerPageShown = WebSiteProperties.NbPagerShown;

            this.Status = _context.ExternalAssociationClientSentStatus
                .Select(x => new DbTableIdNameProperties
                {
                    Id = x.Id,
                    Name = CultureInfo.CurrentCulture.Name == "fr-CA" ? x.Status : x.StatusEN
                }).ToList();


            var isFrench = CultureInfo.CurrentCulture.Name == "fr-CA";
            this.Clients = _context.GetClientSentToExternalAssociation(
                isFrench,
                null,
                null,
                null,
                null,
                1,
                WebSiteProperties.NbResultPerPage).ToList();

            foreach (var client in Clients)
            {
                client.ClientName = client.ClientName.Decode();
            }

            var count = _context.GetClientSentToExternalAssociationCount(
                null,
                null,
                null,
                null).ToList();

            this.CountNotDefined = Convert.ToInt32(count[0].NotDefinedCount);
            this.CountTookInCharge = Convert.ToInt32(count[0].TookInChargeCount);
            this.CountNotTakeInCharge = Convert.ToInt32(count[0].NotTookInChargeCount);
            this.NbPage =
                Convert.ToInt32(
                    Math.Ceiling(
                        Convert.ToDecimal(count[0].TotalResult)
                        /
                        Convert.ToDecimal(WebSiteProperties.NbResultPerPage)
                    )
                );
        }
    }
}