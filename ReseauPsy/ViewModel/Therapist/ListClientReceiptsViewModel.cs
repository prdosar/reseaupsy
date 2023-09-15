using Business;
using Library.Helper;
using ReseauPsy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.ViewModel.Therapist
{
    public class ListClientReceiptsViewModel
    {
        public List<GetListClientReceipt_Result> ClientReceipts { get; set; }
        public int ClientReceiptsCount { get; set; }
        public int NbPage { get; set; }
        public int NbPagerPageShown { get; set; }


        public ListClientReceiptsViewModel(ReseauPsyEntities _context, Business.Therapist therapist)
        {
            this.NbPagerPageShown = WebSiteProperties.NbPagerShown;

            this.ClientReceipts = _context.GetListClientReceipt(
                null,
                null,
                null,
                therapist.Id,
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
                therapist.Id)
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