using Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReseauPsy.Models
{
    public class GetExternalClientFeeDatas
    {
        public decimal PercentageFeeDecimal { get; set; }
        public decimal FixedFee { get; set; }
        private ReseauPsyEntities _context;

        public GetExternalClientFeeDatas(ReseauPsyEntities context)
        {
            this._context = context;
        }

        public void GetMostRecentData()
        {
            var feeData = _context.ExternalClientFee
                .OrderByDescending(x => x.CreateDate)
                .First();

            BindData(feeData);
        }

        public void GetOldData(DateTime date)
        {
            var feeData = _context.ExternalClientFee
                .Where(x => x.CreateDate < date)
                .OrderByDescending(x => x.CreateDate)
                .First();

            BindData(feeData);
        }



        private void BindData(ExternalClientFee feeData)
        {
            this.PercentageFeeDecimal = feeData.ExternalClientPercentageFeeDecimal;
            this.FixedFee = feeData.ExternalClientFixedFee;
        }
    }
}