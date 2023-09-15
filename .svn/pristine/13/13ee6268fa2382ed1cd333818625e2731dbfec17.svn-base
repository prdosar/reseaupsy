using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models
{
    public class GetTaxesData
    {
        public decimal Tps { get; set; }
        public decimal Tvq { get; set; }

        private ReseauPsyEntities _context;


        public GetTaxesData(ReseauPsyEntities context)
        {
            _context = context;
        }

        public void GetMostRecentTaxData()
        {
            var taxData = _context.Taxes
                .OrderByDescending(x => x.ChangedDate)
                .First();

            Tps = taxData.TpsDecimal;
            Tvq = taxData.TvqDecimal;
        }

        public void GetOldTaxData(DateTime date)
        {
            var taxData = _context.Taxes
                .Where(x => x.ChangedDate < date)
                .OrderByDescending(x => x.ChangedDate)
                .First();

            Tps = taxData.TpsDecimal;
            Tvq = taxData.TvqDecimal;
        }
    }
}