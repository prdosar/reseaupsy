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
    public class ListClientReceiptViewModel
    {
        public List<GetListClientReceipt_Result> ClientReceipts { get; set; }
        public int ClientReceiptsCount { get; set; }
        public List<DbTableIdNameProperties> Therapists { get; set; }
        public int NbPage { get; set; }
        public int NbPagerPageShown { get; set; }

        public ListClientReceiptViewModel(ReseauPsyEntities _context)
        {
            var therapistHelper = new TherapistHelper(_context);
            this.Therapists = therapistHelper.GetListTherapistIdName();

            this.NbPagerPageShown = WebSiteProperties.NbPagerShown;

            this.ClientReceipts = _context.GetListClientReceipt(
                null,
                null,
                null,
                null,
                1,
                WebSiteProperties.NbResultPerPage)
                .ToList();

            foreach (var client in ClientReceipts)
            {
                client.ClientName = client.ClientName.Decode();
            }

            this.ClientReceiptsCount = Convert.ToInt32(_context.GetListClientReceiptCount(
                null,
                null,
                null,
                null)
                .ToList()[0]);

            this.NbPage =
                Convert.ToInt32(
                    Math.Ceiling(
                        Convert.ToDecimal(ClientReceiptsCount)
                        /
                        Convert.ToDecimal(WebSiteProperties.NbResultPerPage)
                    )
                );
        }
    }
}